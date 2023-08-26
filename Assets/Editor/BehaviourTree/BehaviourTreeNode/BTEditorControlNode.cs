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
    public class BTEditorControlNode : BTEditorNode
    {
        private VisualElement controlContainer;
        private VisualElement controlData;

        private DropdownField controlDropdown;
        
        protected override void OnDraw()
        {
            IsWarning = true;
            
            controlContainer = LoadVisualElement("Assets/Editor/BehaviourTree/BehaviourTreeNode/BTEditorControlNode.uxml");
            controlData = controlContainer.Q<VisualElement>("control-data");

            controlDropdown = controlContainer.Q<DropdownField>();
            controlDropdown.choices = new List<string>();
            controlDropdown.choices.Add(Define.BehaviourTree.BTControlNodeType.None.ToString());
            controlDropdown.choices.Add(Define.BehaviourTree.BTControlNodeType.Sequence.ToString());
            controlDropdown.choices.Add(Define.BehaviourTree.BTControlNodeType.Selector.ToString());
            controlDropdown.choices.Add(Define.BehaviourTree.BTControlNodeType.If.ToString());
            controlDropdown.choices.Add(Define.BehaviourTree.BTControlNodeType.While.ToString());
            controlDropdown.RegisterValueChangedCallback(evt =>
            {
                if (Enum.TryParse<BehaviourTree.BTControlNodeType>(evt.newValue, out var type))
                {
                    OnChangeChoiceType(type);
                }
            });

            inputPort.Add(CreateInputPort());
        }

        private void OnChangeChoiceType(Define.BehaviourTree.BTControlNodeType _type)
        {
            controlData.Clear();
            IsWarning = _type == BehaviourTree.BTControlNodeType.None;
            
            if (_type == BehaviourTree.BTControlNodeType.None)
            {
                nodeNameLabel.text = nodeTypeName;
                onPortRemoved = null;
                return;
            }

            nodeNameLabel.text = $"{_type.ToString()}Node";

            switch (_type)
            {
                case BehaviourTree.BTControlNodeType.Sequence:
                    controlData.Add(new BTEditorSequenceNode(this));
                    break;
                case BehaviourTree.BTControlNodeType.Selector:
                    controlData.Add(new BTEditorSelectorNode(this));
                    break;
                case BehaviourTree.BTControlNodeType.If:
                    controlData.Add(new BTEditorIfNode(this));
                    break;
                case BehaviourTree.BTControlNodeType.While:
                    controlData.Add(new BTEditorWhileNode(this));
                    break;
            }
        }
    }
}