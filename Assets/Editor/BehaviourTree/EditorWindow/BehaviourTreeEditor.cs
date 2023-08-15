using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class BehaviourTreeEditor : EditorWindow
{
    [SerializeField] private VisualTreeAsset behaviourTreeUxml;
    
    [MenuItem("BehaviourTree/Open Editor")]
    public static void OpenEditor()
    {
        BehaviourTreeEditor wnd = GetWindow<BehaviourTreeEditor>();
        wnd.titleContent = new GUIContent("BehavirouTreeEditor");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        behaviourTreeUxml.CloneTree(root);
    }
}