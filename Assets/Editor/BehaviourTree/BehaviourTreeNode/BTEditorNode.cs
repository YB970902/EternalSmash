using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StaticData;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Assertions;

namespace Editor.BT
{
    public class BTEditorNode : Node
    {
        /// <summary>
        /// 노드 종류의 이름
        /// </summary>
        protected string nodeTypeName;

        private VisualElement nodeMain;
        /// <summary> 입력 포트가 들어갈 요소 </summary>
        private VisualElement inputPortElement;
        /// <summary> 출력 포트가 들어갈 요소 </summary>
        private VisualElement outputPortElement;
        protected VisualElement extension { get; private set; }

        protected Label nodeNameLabel { get; private set; }
        
        /// <summary> 노드 별명 </summary>
        protected TextField nodeNickNameField { get; private set; }

        /// <summary> 입력 포트 정보 </summary>
        private Port inputPort;

        protected BehaviourTreeView treeView;

        /// <summary> 출력 포트 정보 </summary>
        private List<Port> outputPorts;

        public Port InputPort() => inputPort;
        public Port OutputPort(int _index) => outputPorts[_index];

        public System.Action<Port> onPortRemoved;

        /// <summary>
        /// 경고 알람을 띄우는 중인지 여부
        /// </summary>
        private bool isWarning;
        /// <summary>
        /// 경고 알람을 띄운다.
        /// 노드의 색상을 바꾼다.
        /// </summary>
        public bool IsWarning
        {
            get { return isWarning; }
            set
            {
                if (isWarning == value) return;
                
                isWarning = value;
                if (isWarning)
                {
                    nodeMain.AddToClassList("--bt-node-main-warning");
                }
                else
                {
                    nodeMain.RemoveFromClassList("--bt-node-main-warning");
                }
            }
        }

        public BTEditorNode() : base("Assets/Editor/BehaviourTree/BehaviourTreeNode/BTEditorNode.uxml")
        {
            nodeMain = this.Q<VisualElement>("node-main");
            inputPortElement = this.Q<VisualElement>("input-port");
            outputPortElement = this.Q<VisualElement>("output-port");
            extension = this.Q<VisualElement>("extension-content");
            nodeNameLabel = this.Q<Label>("node-name");
            nodeNickNameField = this.Q<TextField>("node-nickname");
            isWarning = false;

            outputPorts = new List<Port>();
        }
        
        public void Init(Vector2 _position, Define.BehaviourTree.BTNodeType _nodeType, BehaviourTreeView _treeView)
        {
            treeView = _treeView;
            nodeTypeName = $"{_nodeType.ToString()}Node";
            
            SetPosition(new Rect(_position, Vector2.zero));
        }

        /// <summary>
        /// StaticData의 정보를 보고 데이터를 세팅한다.
        /// 부모 노드와 연결하거나 자식 노드와 연결하는 등의 내용이 들어있다.
        /// </summary>
        public virtual void SetSdData(SDBehaviourEditorData _data)
        {
            nodeNickNameField.value = _data.NickName;
        }

        public void Draw()
        {
            nodeNameLabel.text = nodeTypeName;

            OnDraw();
        }

        private Port CreatePort(Direction _direction)
        {
            var port = InstantiatePort(Orientation.Vertical, _direction, Port.Capacity.Single, typeof(bool));
            port.portName = string.Empty;
            var label = port.Q<Label>("type");
            label.style.marginLeft = 0;
            label.style.marginRight = 0;

            return port;
        }
        
        protected void MakeInputPort()
        {
            var port = CreatePort(Direction.Input);
            
            inputPort = port;
            inputPortElement.Add(port);
        }
        
        public Port MakeOutputPort()
        {
            var port = CreatePort(Direction.Output);
            
            outputPorts.Add(port);
            outputPortElement.Add(port);

            return port;
        }

        public void RemoveOutputPort(Port _port)
        {
            if (outputPorts.Contains(_port) == false) return;
            outputPorts.Remove(_port);
            treeView.DeleteElements(_port.connections);
        }

        protected VisualElement LoadVisualElement(string _path)
        {
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(_path).Instantiate();
            extension.Add(visualTree);
            return visualTree;
        }

        /// <summary>
        /// 노드가 생성되고 그려질때 호출될 함수.
        /// </summary>
        protected virtual void OnDraw()
        {
            
        }
        
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("입력 포트 연결 해제", actionEvent => DisconnectInputPort());
            evt.menu.AppendAction("출력 포트 연결 해제", actionEvent => DisconnectOutputPorts());

            base.BuildContextualMenu(evt);
        }

        public int GetNodeID()
        {
            return parent.IndexOf(this);
        }

        public int GetParentNodeID()
        {
            var edge = inputPort.connections.First();
            return (edge.output.node as BTEditorNode)?.GetNodeID() ?? Define.BehaviourTree.InvalidID;
        }

        public int GetChildNodeID(int _index)
        {
            if (_index < 0 || _index >= outputPorts.Count) return Define.BehaviourTree.InvalidID;
            var edge = outputPorts[_index].connections.First();
            return (edge.input.node as BTEditorNode)?.GetNodeID() ?? Define.BehaviourTree.InvalidID;
        }

        public int GetConnectedNodeID(Port _port)
        {
            if (_port == null) return Define.BehaviourTree.InvalidID;
            Assert.IsTrue(_port.connected, "연결되지 않은 노드가 있습니다.");
            var edge = _port.connections.First();
            return (edge.input.node as BTEditorNode)?.GetNodeID() ?? Define.BehaviourTree.InvalidID;
        }

        /// <summary>
        /// 행동트리 노드 인스턴스화를 위한 BT데이터 값 생성
        /// </summary>
        public virtual SDBehaviourEditorData CreateSDBehaviourTreeData()
        {
            var result = new SDBehaviourEditorData();
            result.NickName = nodeNickNameField.text;
            var position = GetPosition().position;
            result.X = position.x;
            result.Y = position.y;
            return result;
        }
        
        #region Port

        public void DisconnectAllPorts()
        {
            DisconnectInputPort();
            DisconnectOutputPorts();
        }

        /// <summary>
        /// 입력포트가 삭제된경우
        /// </summary>
        private void DisconnectInputPort()
        {
            // 입력포트와 연결된 간선을 모두 삭제한다.
            if(inputPort != null && inputPort.connected) treeView.DeleteElements(inputPort.connections);
        }

        /// <summary>
        /// 출력 포트가 삭제된경우
        /// </summary>
        private void DisconnectOutputPorts()
        {
            for (int i = 0, count = outputPorts.Count; i < count; ++i)
            {
                if (outputPorts[i].connected == false) continue;
                // 연결된 모든 간선을 삭제한다.
                treeView.DeleteElements(outputPorts[i].connections);
            }
        }

        protected override void OnPortRemoved(Port _port)
        {
            base.OnPortRemoved(_port);
            onPortRemoved?.Invoke(_port);
        }
        
        #endregion

        public override void OnSelected()
        {
            base.OnSelected();
            this.Q<VisualElement>("node-main").AddToClassList("--bt-node-main-selected");
        }

        public override void OnUnselected()
        {
            base.OnUnselected();
            this.Q<VisualElement>("node-main").RemoveFromClassList("--bt-node-main-selected");
        }
    }
}