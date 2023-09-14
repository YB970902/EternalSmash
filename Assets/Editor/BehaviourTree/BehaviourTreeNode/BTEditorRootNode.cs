using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Define;
using StaticData;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;

namespace Editor.BT
{
    public class BTEditorRootNode : BTEditorNode
    {
        protected override void OnDraw()
        {
            outputPort.Add(CreateOutputPort());
        }

        public override SDBehaviourEditorData CreateSDBehaviourTreeData()
        {
            var result = base.CreateSDBehaviourTreeData();
            result.Type = BehaviourTree.BTEditorDataType.Root;
            result.ID = GetNodeID();
            result.ChildID = GetChildNodeID(0);
            return result;
        }
    }
}