using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;

namespace Editor.BT
{
    public class BTEditorNode : Node
    {
        /// <summary>
        /// 노드 종류의 이름
        /// </summary>
        protected string nodeTypeName;

        private VisualElement nodeMain;
        protected VisualElement inputPort { get; private set; }
        protected VisualElement outputPort { get; private set; }
        protected VisualElement extension { get; private set; }

        protected Label nodeNameLabel { get; private set; }

        private BehaviourTreeView treeView;

        private List<Port> inputPorts = new List<Port>();
        protected List<Port> outputPorts = new List<Port>();

        public System.Action<Port> onPortRemoved;

        private bool isWarning;
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
            inputPort = this.Q<VisualElement>("input-port");
            outputPort = this.Q<VisualElement>("output-port");
            extension = this.Q<VisualElement>("extension-content");
            nodeNameLabel = this.Q<Label>("node-name");
            isWarning = false;
        }
        
        public void Init(Vector2 _position, Define.BehaviourTree.BTNodeType _nodeType, BehaviourTreeView _treeView)
        {
            treeView = _treeView;
            nodeTypeName = $"{_nodeType.ToString()}Node";

            SetPosition(new Rect(_position, Vector2.zero));

            List<int> list = new List<int>() { 1, 2, 3, 4, 5 };
            list.Add(6);
            var readList = list.AsReadOnly();
            var item = readList[0];
        }
        
        public void Draw()
        {
            nodeNameLabel.text = nodeTypeName;

            OnDraw();
        }

        protected Port CreateInputPort()
        {
            var port = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
            port.portName = string.Empty;
            port.Q<Label>("type").style.marginLeft = 0;
            port.Q<Label>("type").style.marginRight = 0;
            
            inputPorts.Add(port);
            
            return port;
        }
        
        public Port CreateOutputPort()
        {
            var port = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
            port.portName = string.Empty;
            port.Q<Label>("type").style.marginLeft = 0;
            port.Q<Label>("type").style.marginRight = 0;
            
            outputPorts.Add(port);
            
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
            evt.menu.AppendAction("Disconnect Input Ports", actionEvent => DisconnectInputPorts());
            evt.menu.AppendAction("Disconnect Output Ports", actionEvent => DisconnectOutputPorts());

            base.BuildContextualMenu(evt);
        }

        public int GetNodeID()
        {
            return parent.IndexOf(this);
        }

        public int GetParentNodeID()
        {
            var edge = inputPorts[0].connections.First();
            return (edge.output.node as BTEditorNode)?.GetNodeID() ?? Define.BehaviourTree.InvalidID;
        }

        protected int GetChildNodeID(int _index)
        {
            if (_index < 0 || _index >= outputPorts.Count) return Define.BehaviourTree.InvalidID;
            var edge = outputPorts[_index].connections.First();
            return (edge.input.node as BTEditorNode)?.GetNodeID() ?? Define.BehaviourTree.InvalidID;
        }

        public int GetConnectedNodeID(Port _port)
        {
            if (_port == null) return Define.BehaviourTree.InvalidID;
            var edge = _port.connections.First();
            return (edge.input.node as BTEditorNode)?.GetNodeID() ?? Define.BehaviourTree.InvalidID;
        }
        
        /// <summary>
        /// 행동트리 노드 인스턴스화를 위한 BT데이터 값 생성
        /// </summary>
        public virtual BTData CreateBTData()
        {
            return null;
        }
        
        #region Port

        public void DisconnectAllPorts()
        {
            DisconnectInputPorts();
            DisconnectOutputPorts();
        }

        private void DisconnectInputPorts()
        {
            for (int i = 0, count = inputPorts.Count; i < count; ++i)
            {
                if (inputPorts[i].connected == false) continue;
                treeView.DeleteElements(inputPorts[i].connections);
            }
        }

        private void DisconnectOutputPorts()
        {
            for (int i = 0, count = outputPorts.Count; i < count; ++i)
            {
                if (outputPorts[i].connected == false) continue;
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