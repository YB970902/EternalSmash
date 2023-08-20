using System;
using System.Collections;
using System.Collections.Generic;
using Define;
using Editor.Util;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;

namespace Editor.BT
{
    public class BTEditorSelectorNode : VisualElement
    {
        private MultiPortController multiPortController;
        
        public BTEditorSelectorNode(BTEditorNode _node)
        {
            multiPortController = new MultiPortController();
            multiPortController.Init(_node);
            Add(multiPortController);
        }
    }
}