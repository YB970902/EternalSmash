using System;
using System.Collections;
using System.Collections.Generic;
using Codice.Client.GameUI.Checkin;
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
    public class BTEditorExecuteNode : BTEditorNode
    {
        private Define.BehaviourTree.Execute exeucteTag;
        protected override void OnDraw()
        {
            IsWarning = true;
            
            LoadVisualElement("Assets/Editor/BehaviourTree/BehaviourTreeNode/BTEditorExecuteNode.uxml");
            MakeInputPort();

            var dropdown = this.Q<DropdownField>();
            dropdown.choices = new List<string>();
            for (var i = Define.BehaviourTree.Execute.None; i < Define.BehaviourTree.Execute.End; ++i)
            {
                dropdown.choices.Add(i.ToString());
            }
            dropdown.RegisterValueChangedCallback(OnValueChange);
        }

        private void OnValueChange(ChangeEvent<string> _evt)
        {
            if (Enum.TryParse<Define.BehaviourTree.Execute>(_evt.newValue, out var tag) == false) return;

            IsWarning = tag == BehaviourTree.Execute.None;
            exeucteTag = tag;
        }

        public override SDBehaviourEditorData CreateSDBehaviourTreeData()
        {
            var result = base.CreateSDBehaviourTreeData();
            result.Type = BehaviourTree.BTEditorDataType.Execute;
            result.NodeID = GetNodeID();
            result.ParentID = GetParentNodeID();
            result.ExecuteType = exeucteTag;
            return result;
        }

        public override void SetSdData(SDBehaviourEditorData _data)
        {
            base.SetSdData(_data);
            
            var dropdown = this.Q<DropdownField>();
            dropdown.value = _data.ExecuteType.ToString();
        }
    }
}