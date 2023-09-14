using System;
using System.Collections;
using System.Collections.Generic;
using Define;
using Editor.Util;
using StaticData;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;

namespace Editor.BT
{
    public class BTEditorWhileNode : VisualElement, IEditorControlNode
    {
        private BTEditorNode node;
        private Port port;
        
        public int repeatCount { get; private set; }
        
        public BTEditorWhileNode(BTEditorNode _node)
        {
            node = _node;
            
            var whileNodeUxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/BehaviourTree/BehaviourTreeNode/ControlNode/BTEditorWhileNode.uxml");
            whileNodeUxml.CloneTree(this);

            var repeatCountField = this.Q<IntegerField>();
            repeatCountField.RegisterValueChangedCallback(OnValueChange);
            repeatCount = 0;

            var portContainer = this.Q<VisualElement>("port-container");
            portContainer.Add(_node.CreateOutputPort());
        }

        private void OnValueChange(ChangeEvent<int> _evt)
        {
            repeatCount = _evt.newValue;
        }

        public void CreateSDBehaviourTreeData(ref SDBehaviourEditorData _data)
        {
            _data.Type = BehaviourTree.BTEditorDataType.While;
            _data.ID = node.GetNodeID();
            _data.ParentID = node.GetParentNodeID();
            _data.ChildID = node.GetConnectedNodeID(port);
            _data.RepeatCount = repeatCount;
        }
    }
}