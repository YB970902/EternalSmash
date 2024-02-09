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
    public class BTEditorSelectorNode : VisualElement, IEditorControlNode
    {
        private BTEditorNode node;
        private MultiPortController multiPortController;
        
        public BTEditorSelectorNode(BTEditorNode _node)
        {
            node = _node;
            multiPortController = new MultiPortController();
            multiPortController.Init(_node);
            Add(multiPortController);
        }
        
        public void CreateSDBehaviourTreeData(ref SDBehaviourEditorData _data)
        {
            var children = new List<int>(multiPortController.Count);
            for (int i = 0, count = multiPortController.Count; i < count; ++i)
            {
                children.Add(multiPortController.GetConnectedNodeID(i));
            }
            
            _data.Type = BehaviourTree.BTEditorDataType.Selector;
            _data.NodeID = node.GetNodeID();
            _data.ParentID = node.GetParentNodeID();
            _data.ChildrenID = children;
        }

        public void SetConnectData(SDBehaviourEditorData _data, BehaviourTreeView _treeView)
        {
            var index = 0;
            foreach(var childID in _data.ChildrenID)
            {
                multiPortController.OnClickAddButton(null);
                var childNode = _treeView.GetNodeByIndex(childID).Q<BTEditorNode>();
                _treeView.Add(multiPortController.Port(index++).ConnectTo(childNode.InputPort()));
            }
        }
    }
}