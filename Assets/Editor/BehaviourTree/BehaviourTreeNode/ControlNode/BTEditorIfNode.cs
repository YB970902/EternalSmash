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
    public class BTEditorIfNode : VisualElement, IEditorControlNode
    {
        private BTEditorNode node;
        
        private Port truePort;
        private Port falsePort;

        private BehaviourTree.Conditional conditionalType;
        
        public BTEditorIfNode(BTEditorNode _node)
        {
            node = _node;
            
            var ifNodeUxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/BehaviourTree/BehaviourTreeNode/ControlNode/BTEditorIfNode.uxml");
            ifNodeUxml.CloneTree(this);

            var dropdown = this.Q<DropdownField>();
            dropdown.choices = new List<string>();
            for (var i = Define.BehaviourTree.Conditional.None; i < Define.BehaviourTree.Conditional.End; ++i)
            {
                dropdown.choices.Add(i.ToString());
            }

            dropdown.RegisterValueChangedCallback(OnDropdownChanged);
            
            truePort = _node.CreateOutputPort();
            falsePort = _node.CreateOutputPort();
            this.Q<VisualElement>("true-node").Add(truePort);
            this.Q<VisualElement>("false-node").Add(falsePort);
        }

        private void OnDropdownChanged(ChangeEvent<string> _evt)
        {
            if (Enum.TryParse<Define.BehaviourTree.Conditional>(_evt.newValue, out var type) == false) return;

            conditionalType = type;
        }

        public void CreateSDBehaviourTreeData(ref SDBehaviourEditorData _data)
        {
            _data.Type = BehaviourTree.BTEditorDataType.If;
            _data.NodeID = node.GetNodeID();
            _data.ParentID = node.GetParentNodeID();
            _data.TrueNodeID = node.GetConnectedNodeID(truePort);
            _data.FalseNodeID = node.GetConnectedNodeID(falsePort);
            _data.ConditionalFuncType = conditionalType;
        }

        public void SetConnectData(SDBehaviourEditorData _data, BehaviourTreeView _treeView)
        {
            var trueNode = _treeView.GetNodeByIndex(_data.TrueNodeID).Q<BTEditorNode>();
            var falseNode = _treeView.GetNodeByIndex(_data.FalseNodeID).Q<BTEditorNode>();
            _treeView.Add(truePort.ConnectTo(trueNode.InputPort));
            _treeView.Add(falsePort.ConnectTo(falseNode.InputPort));
            
            var dropdown = this.Q<DropdownField>();
            dropdown.value = _data.ConditionalFuncType.ToString();
        }
    }
}