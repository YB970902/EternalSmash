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
    public class BTEditorExecuteNode : BTEditorNode
    {
        private Define.BehaviourTree.BTExecute exeucteTag;
        protected override void OnDraw()
        {
            IsWarning = true;
            
            LoadVisualElement("Assets/Editor/BehaviourTree/BehaviourTreeNode/BTEditorExecuteNode.uxml");
            inputPort.Add(CreateInputPort());

            var dropdown = this.Q<DropdownField>();
            dropdown.choices = new List<string>();
            for (var i = Define.BehaviourTree.BTExecute.None; i < Define.BehaviourTree.BTExecute.End; ++i)
            {
                dropdown.choices.Add(i.ToString());
            }
            dropdown.RegisterValueChangedCallback(OnValueChange);
        }

        private void OnValueChange(ChangeEvent<string> _evt)
        {
            if (Enum.TryParse<Define.BehaviourTree.BTExecute>(_evt.newValue, out var tag) == false) return;

            IsWarning = tag == BehaviourTree.BTExecute.None;
            exeucteTag = tag;
        }
    }
}