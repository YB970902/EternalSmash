using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;

namespace Editor.BT
{
    public class BTEditorNode : Node
    {
        /// <summary> 노드의 이름. 어떤 노드인지 구분하기 위해 존재한다. </summary>
        private string nodeName;

        public BTEditorNode()
        {
            nodeName = "EditorNode";

            Init();
        }
        
        private void Init()
        {
            var nodeStyleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviourTree/BehaviourTreeNode/BTEditorNode.uss");
            styleSheets.Add(nodeStyleSheet);
            
            mainContainer.AddToClassList("--bt-node-background");
            
            // 사용하지 않는 컨테이너 제거
            titleButtonContainer.RemoveFromHierarchy();
            inputContainer.RemoveFromHierarchy();
            outputContainer.RemoveFromHierarchy();
            
            // 부모노드 Port 추가
            var parentPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
            parentPort.portName = "ParentNode";
            parentPort.AddToClassList("--bt-node-port");
            mainContainer.Insert(0, parentPort);

            // 노드의 별명 필드 추가
            var nodeNickNameField = new TextField()
            {
                value = nodeName,
                bindingPath = nodeName.GetType().Name
            };
            nodeNickNameField.AddToClassList("--bt-node-input-field");
            
            titleContainer.Insert(0, nodeNickNameField);
            
            // 노드의 이름 라벨 추가
            var nodeNameLabel = new Label()
            {
                text = "BTEditorNode",
            };
            nodeNameLabel.AddToClassList("--bt-node-name");
            
            extensionContainer.Add(nodeNameLabel);
            extensionContainer.AddToClassList("--bt-node-extension-background");
            
            RefreshExpandedState();
        }
    }
}