using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Util
{
    public class MultiPort : VisualElement
    {
        private MultiPortController controller;
        
        public MultiPort()
        {
            var multiPortUxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/BehaviourTree/MultiPort/MultiPort.uxml");
            Add(multiPortUxml.Instantiate());
            
            this.Q<Button>("btn-remove").RegisterCallback<ClickEvent>(OnClickRemove);
        }

        public void Init(MultiPortController _controller, Port _port)
        {
            var portContainer = this.Q<VisualElement>("output-port");
            controller = _controller;
            portContainer.Add(_port);
        }

        private void OnClickRemove(ClickEvent _evt)
        {
            controller.OnPortRemoved(this);
            RemoveFromHierarchy();
        }
    }
}