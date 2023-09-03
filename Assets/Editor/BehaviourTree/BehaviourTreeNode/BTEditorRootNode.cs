using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public override BTData CreateBTData()
        {
            var result = new BTRootData(GetNodeID(), GetChildNodeID(0));
            return result;
        }
    }
}