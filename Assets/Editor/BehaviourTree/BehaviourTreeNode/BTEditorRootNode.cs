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
        public override void SetSdData(SDBehaviourEditorData _data)
        {
            base.SetSdData(_data);
            
            var port = OutputPort(0);
            var targetNode = treeView.GetNodeByIndex(_data.ChildID).Q<BTEditorNode>();
            treeView.Add(port.ConnectTo(targetNode.InputPort()));
        }

        protected override void OnDraw()
        {
            MakeOutputPort();
        }

        public override SDBehaviourEditorData CreateSDBehaviourTreeData()
        {
            var result = base.CreateSDBehaviourTreeData();
            result.Type = BehaviourTree.BTEditorDataType.Root;
            result.NodeID = GetNodeID();
            result.ChildID = GetChildNodeID(0);
            return result;
        }
    }
}