using System.Collections;
using System.Collections.Generic;
using Editor.BT;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Util
{
    public class MultiPortController : VisualElement
    {
        private VisualElement container;
        private List<MultiPort> portList;
        private BTEditorNode node;

        public MultiPortController()
        {
            var multiportController = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/BehaviourTree/MultiPort/MultiPortController.uxml");
            multiportController.CloneTree(this);
            container = this.Q<VisualElement>("multi-port-container");
            portList = new List<MultiPort>();
            this.Q<Button>("add-button").RegisterCallback<ClickEvent>(OnClickAddButton);
        }

        public void Init(BTEditorNode _node)
        {
            node = _node;
        }

        private void OnClickAddButton(ClickEvent _evt)
        {
            var port = new MultiPort();
            port.Init(this, node.CreateOutputPort());
            portList.Add(port);
            container.Insert(container.childCount - 1, port);
        }

        public void OnPortRemoved(MultiPort _port)
        {
            portList.Remove(_port);
        }
    }
}
