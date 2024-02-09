using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Define;
using StaticData;

namespace Editor.BT
{
    /// <summary>
    /// GraphView로 노드를 보여주는 스크립트.
    /// 노드를 생성, 삭제하는 기능이 들어있다.
    /// </summary>
    public class BehaviourTreeView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<BehaviourTreeView, GraphView.UxmlTraits>
        {
        }

        private List<BTEditorNode> editorNodeList = new List<BTEditorNode>();

        public BehaviourTreeView()
        {
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(CreateNodeContextualMenu());

            styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviourTree/BehaviourTreeView.uss"));

            OnElementDestroyed();
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePort = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort.node == port.node) return;

                if (startPort.direction == port.direction) return;

                compatiblePort.Add(port);
            });
            
            return compatiblePort;
        }

        // 우클릭시 메뉴가 나오는 Manipulator
        private IManipulator CreateNodeContextualMenu()
        {
            var menuManipulator = new ContextualMenuManipulator(
                _event =>
                {
                    _event.menu.AppendAction("루트 노드 추가", _actionEvent => AddNode(BehaviourTree.BTNodeType.Root, _actionEvent.eventInfo.localMousePosition));
                    _event.menu.AppendAction("컨트롤 노드 추가", _actionEvent => AddNode(BehaviourTree.BTNodeType.Control, _actionEvent.eventInfo.localMousePosition));
                    _event.menu.AppendAction("행동 노드 추가", _actionEvent => AddNode(BehaviourTree.BTNodeType.Execute, _actionEvent.eventInfo.localMousePosition));
                });

            return menuManipulator;
        }

        private void AddNode(Define.BehaviourTree.BTNodeType _nodeType, Vector2 _position)
        {
            _position.x = (_position.x - contentViewContainer.worldBound.xMin) / scale;
            _position.y = (_position.y - contentViewContainer.worldBound.yMin) / scale;

            CreateEditorNode(_nodeType, _position);
        }

        private BTEditorNode CreateEditorNode(BehaviourTree.BTNodeType _nodeType, Vector2 _position)
        {
            var type = System.Type.GetType($"Editor.BT.BTEditor{_nodeType.ToString()}Node");
            var node = Activator.CreateInstance(type) as BTEditorNode;
            node.Init(_position, _nodeType, this);
            node.Draw();
            
            editorNodeList.Add(node);
            
            AddElement(node);

            return node;
        }

        public Node GetNodeByIndex(int _index)
        {
            return editorNodeList[_index];
        }

        /// <summary>
        /// 현재 노드 정보를 비우고 새로 만든다.
        /// </summary>
        public void New()
        {
            ClearAllNode();
        }
        
        /// <summary>
        /// 노드 정보를 저장한다.
        /// </summary>
        public void Save(string _name)
        {
            StaticDataManager.Save(editorNodeList.Select(_node => _node.CreateSDBehaviourTreeData()).ToList(), _name);
        }

        /// <summary>
        /// 노드 정보를 불러온다.
        /// </summary>
        public void Load(string _name)
        {
            AssetDatabase.Refresh();
            var data = StaticDataManager.Load<SDBehaviourEditorData>(_name);
            var orderedData = data.DataList.OrderBy(_ => _.NodeID).ToList();
            var editorNodes = new List<BTEditorNode>(orderedData.Count);
            
            foreach (var sdData in orderedData)
            {
                editorNodes.Add(CreateEditorNode(sdData.Type.ToBtNodeType(), new Vector2(sdData.X, sdData.Y)));
            }

            for (int i = 0, count = editorNodes.Count; i < count; ++i)
            {
                editorNodes[i].SetSdData(orderedData[i]);
            }
        }

        /// <summary>
        /// 모든 노드를 삭제한다.
        /// </summary>
        private void ClearAllNode()
        {
            foreach (var node in editorNodeList)
            {
                RemoveElement(node);
            }
        }
        
        #region Callbacks

        private void OnElementDestroyed()
        {
            deleteSelection = (operationName, askUser) =>
            {
                var edgeType = typeof(Edge);

                var nodesToDelete = new List<BTEditorNode>();
                var edgesToDelete = new List<Edge>();

                foreach (var selectedElement in selection)
                {
                    if (selectedElement is BTEditorNode node)
                    {
                        editorNodeList.Remove(node);
                        nodesToDelete.Add(node);
                        continue;
                    }

                    if (selectedElement.GetType() != edgeType) continue;
                    
                    Edge edge = (Edge) selectedElement;
                    edgesToDelete.Add(edge);
                }

                DeleteElements(edgesToDelete);

                foreach (BTEditorNode nodeToDelete in nodesToDelete)
                {
                    nodeToDelete.DisconnectAllPorts();
                    RemoveElement(nodeToDelete);
                }
            };
        }
        
        #endregion
    }
}