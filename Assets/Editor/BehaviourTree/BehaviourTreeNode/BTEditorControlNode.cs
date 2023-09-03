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
    public interface IEditorControlNode
    {
        public BTData CreateBTData();
    }
    public class BTEditorControlNode : BTEditorNode
    {
        private VisualElement controlContainer;
        private VisualElement controlData;

        private DropdownField controlDropdown;

        private IEditorControlNode controlNode;
        
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
                controlNode = null;
                nodeNameLabel.text = nodeTypeName;
                onPortRemoved = null;
                return;
            }

            nodeNameLabel.text = $"{_type.ToString()}Node";

            VisualElement controlElement;
            
            switch (_type)
            {
                case BehaviourTree.BTControlNodeType.Sequence:
                    controlElement = new BTEditorSequenceNode(this);
                    controlData.Add(controlElement);
                    break;
                case BehaviourTree.BTControlNodeType.Selector:
                    controlElement = new BTEditorSelectorNode(this); 
                    controlData.Add(controlElement);
                    break;
                case BehaviourTree.BTControlNodeType.If:
                    controlElement = new BTEditorIfNode(this);
                    controlData.Add(controlElement);
                    break;
                case BehaviourTree.BTControlNodeType.While:
                    controlElement = new BTEditorWhileNode(this);
                    controlData.Add(controlElement);
                    break;
                default: return;
            }
            
            controlNode = controlElement as IEditorControlNode;
        }

        public override BTData CreateBTData()
        {
            return controlNode.CreateBTData();
        }
    }
}