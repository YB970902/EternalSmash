using System;
using System.Collections;
using System.Collections.Generic;
using Define;
using Editor.Util;
using NUnit.Framework;
using StaticData;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;

namespace Editor.BT
{
    public class BTEditorSequenceNode : VisualElement, IEditorControlNode
    {
        private BTEditorNode node;
        private MultiPortController multiPortController;

        public BTEditorSequenceNode(BTEditorNode _node)
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

            _data.Type = BehaviourTree.BTEditorDataType.Sequence;
            _data.ID = node.GetNodeID();
            _data.ParentID = node.GetParentNodeID();
            _data.ChildrenID = children;
        }
    }
}