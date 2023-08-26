using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Define;

namespace Editor.BT
{
    public class BehaviourTreeView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<BehaviourTreeView, GraphView.UxmlTraits>
        {
        }

        public BehaviourTreeView()
        {
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(CreateNodeContextualMenu());

            styleSheets.Add(
                AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviourTree/BehaviourTreeView.uss"));

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
                    _event.menu.AppendAction("AddRootNode", _actionEvent => AddElement(CreateNode(BehaviourTree.BTNodeType.Root, _actionEvent.eventInfo.localMousePosition)));
                    _event.menu.AppendAction("AddControlNode", _actionEvent => AddElement(CreateNode(BehaviourTree.BTNodeType.Control, _actionEvent.eventInfo.localMousePosition)));
                    _event.menu.AppendAction("AddExecuteNode", _actionEvent => AddElement(CreateNode(BehaviourTree.BTNodeType.Execute, _actionEvent.eventInfo.localMousePosition)));
                });

            return menuManipulator;
        }

        private BTEditorNode CreateNode(Define.BehaviourTree.BTNodeType _nodeType, Vector2 _position)
        {
            _position.x = (_position.x - contentViewContainer.worldBound.x) / scale;
            _position.y = (_position.y - contentViewContainer.worldBound.y) / scale;

            var type = System.Type.GetType($"Editor.BT.BTEditor{_nodeType.ToString()}Node");
            var node = Activator.CreateInstance(type) as BTEditorNode;
            node.Init(_position, _nodeType, this);
            node.Draw();
            return node;
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