using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Editor.BT;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Util
{
    public class MultiPortController : VisualElement
    {
        private VisualElement container;
        private List<MultiPort> portList;
        private BTEditorNode node;

        public Port Port(int _index) => portList[_index].Port;

        public int Count => portList.Count;

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
            node.onPortRemoved = OnPortRemoved;
        }

        public void OnClickAddButton(ClickEvent _evt)
        {
            var port = new MultiPort();
            port.Init(this, node.CreateOutputPort());
            portList.Add(port);
            container.Insert(container.childCount - 1, port);
        }

        public void OnPortRemoved(MultiPort _port)
        {
            portList.Remove(_port);
            node.RemoveOutputPort(_port.Port);
        }

        private void OnPortRemoved(UnityEditor.Experimental.GraphView.Port _port)
        {
            var multiPort = portList.Find(_ => _.Port == _port);
            
            if (multiPort == null) return;

            multiPort.Remove();
        }

        public int GetConnectedNodeID(int _index)
        {
            if (_index < 0 || _index >= portList.Count) return Define.BehaviourTree.InvalidID;
            var edge = portList[_index].Port.connections.First();
            return (edge.input.node as BTEditorNode)?.GetNodeID() ?? Define.BehaviourTree.InvalidID;
        }
    }
}
