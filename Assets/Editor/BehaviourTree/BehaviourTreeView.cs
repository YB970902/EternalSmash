using System;
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

            AddElement(CreateNode(BehaviourTree.BTNodeType.Execute, Vector2.zero));
        }

        // 우클릭시 메뉴가 나오는 Manipulator
        private IManipulator CreateNodeContextualMenu()
        {
            var menuManipulator = new ContextualMenuManipulator(
                _event =>
                {
                    _event.menu.AppendAction("AddRootNode", _actionEvent => AddElement(CreateNode(BehaviourTree.BTNodeType.Root, _actionEvent.eventInfo.localMousePosition)));
                });

            return menuManipulator;
        }

        private BTEditorNode CreateNode(Define.BehaviourTree.BTNodeType _nodeType, Vector2 _position)
        {
            var type = System.Type.GetType($"Editor.BT.BTEditor{_nodeType.ToString()}Node");
            var node = Activator.CreateInstance(type) as BTEditorNode;
            node.Init(_position, _nodeType);
            node.Draw();
            return node;
        }
    }
}