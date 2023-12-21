using Editor.BT;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public class BehaviourTreeEditor : EditorWindow
{
    [SerializeField] private VisualTreeAsset behaviourTreeUxml;

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

        treeView = root.Q<BehaviourTreeView>();

        root.Q<Button>("btn_new").clicked += OnClickNew;
        root.Q<Button>("btn_open").clicked += OnClickOpen;
        root.Q<Button>("btn_save").clicked += OnClickSave;
    }

    private void OnClickNew()
    {
        treeView.Load("");
    }
    
    private void OnClickSave()
    {
        treeView.Save("default");
    }

    private void OnClickOpen()
    {
        treeView.Load("default");
    }
}