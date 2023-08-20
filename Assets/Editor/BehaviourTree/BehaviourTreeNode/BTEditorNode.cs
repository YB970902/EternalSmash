using System.Collections;
using System.Collections.Generic;
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

        protected VisualElement inputPort { get; private set; }
        protected VisualElement outputPort { get; private set; }
        protected VisualElement extension { get; private set; }

        protected Label nodeNameLabel { get; private set; }

        public BTEditorNode() : base("Assets/Editor/BehaviourTree/BehaviourTreeNode/BTEditorNode.uxml")
        {
            inputPort = this.Q<VisualElement>("input-port");
            outputPort = this.Q<VisualElement>("output-port");
            extension = this.Q<VisualElement>("extension-content");
            nodeNameLabel = this.Q<Label>("node-name");
        }
        
        public virtual void Init(Vector2 _position, Define.BehaviourTree.BTNodeType _nodeType)
        {
            nodeTypeName = $"{_nodeType.ToString()}Node";
            
            SetPosition(new Rect(_position, Vector2.zero));
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
            return port;
        }
        
        public Port CreateOutputPort()
        {
            var port = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
            port.portName = string.Empty;
            port.Q<Label>("type").style.marginLeft = 0;
            port.Q<Label>("type").style.marginRight = 0;
            return port;
        }

        protected VisualElement LoadVisualElement(string _path)
        {
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(_path).Instantiate();
            extension.Add(visualTree);
            return visualTree;
        }

        protected void AddToExtension(VisualElement _element)
        {
            extension.Add(_element);
        }

        /// <summary>
        /// 노드가 생성되고 그려질때 호출될 함수.
        /// </summary>
        protected virtual void OnDraw()
        {
            
        }
    }
}