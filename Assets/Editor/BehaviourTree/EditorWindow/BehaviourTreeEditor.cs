using Editor.BT;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public class BehaviourTreeEditor : EditorWindow
{
    [SerializeField] private VisualTreeAsset behaviourTreeUxml;
    [SerializeField] private VisualTreeAsset inspectorUxml;

    private BehaviourTreeView treeView;
    
    [MenuItem("BehaviourTree/Open Editor")]
    public static void OpenEditor()
    {
        BehaviourTreeEditor wnd = GetWindow<BehaviourTreeEditor>();
        wnd.titleContent = new GUIContent("BehaviourTreeEditor");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        behaviourTreeUxml.CloneTree(root);

        var inspectorView = root.Q<VisualElement>("inspector-view"); 
        inspectorUxml.CloneTree(inspectorView);

        treeView = root.Q<BehaviourTreeView>();

        var btnSave = inspectorView.Q<Button>("btn-save");
        btnSave.RegisterCallback<ClickEvent>(OnClickSave);
    }

    private void OnClickSave(ClickEvent _evt)
    {
        treeView.Save();
    }
}