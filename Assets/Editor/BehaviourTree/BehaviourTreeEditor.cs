using System.Linq;
using Editor.BT;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 행동트리 에디터의 메인
/// BehaviourTreeView를 띄운다.
/// </summary>
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
        treeView.New();
    }
    
    private void OnClickSave()
    {
        var path = EditorUtility.SaveFilePanel("저장하기", "Assets/StaticData/SDBehaviourEditorData", "New BehaviourTree", "bytes");
        
        // 취소했을 경우 반환값이 Empty이므로, 반환한다.
        if (path == string.Empty) return;
        
        var fileName = path.Split("/").Last().Split(".").First();
        treeView.Save(fileName);
    }

    private void OnClickOpen()
    {
        var path = EditorUtility.OpenFilePanel("불러오기", "Assets/StaticData/SDBehaviourEditorData", "bytes");
        
        // 취소했을 경우 반환값이 Empty이므로, 반환한다.
        if (path == string.Empty) return;
        
        var fileName = path.Split("/").Last().Split(".").First();
        treeView.Load(fileName);
    }
}