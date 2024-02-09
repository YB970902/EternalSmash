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
        
        public Port Port { get; private set; }
        
        public MultiPort()
        {
            var multiPortUxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/BehaviourTree/MultiPort/MultiPort.uxml");
            Add(multiPortUxml.Instantiate());
            
            this.Q<Button>("btn-remove").RegisterCallback<ClickEvent>(OnClickRemove);
        }

        public void Init(MultiPortController _controller, Port _port)
        {
            Port = _port;
            controller = _controller;
            
            var portContainer = this.Q<VisualElement>("output-port");
            portContainer.Add(_port);
        }

        private void OnClickRemove(ClickEvent _evt)
        {
            Remove();
        }

        public void Remove()
        {
            controller.OnPortRemoved(this);
            RemoveFromHierarchy();
        }
    }
}