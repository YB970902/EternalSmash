using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BehaviourTree를 생성해주는 빌더.
/// 빌더를 하나만 인스턴스 해놓으면 여러종류의 BehaviourTree를 만들 수 있다.
/// </summary>
public class BTBuilder
{
    private List<BTNodeBase> nodeBases;
    private List<BTData> nodeDatas;

    /// <summary> 결과로 반환할 컨트롤러 </summary>
    private BTController controller;
    
    public BTBuilder()
    {
        nodeBases = new List<BTNodeBase>();
        nodeDatas = new List<BTData>();
        controller = null;
    }

    /// <summary>
    /// 모든 데이터를 지운다.
    /// </summary>
    public void Clear()
    {
        nodeBases.Clear();
        nodeDatas.Clear();
        controller = null;
    }
    
    /// <summary>
    /// 입력된 데이터를 기반으로 노드를 만든 후에
    /// </summary>
    /// <returns></returns>
    public BTController Build()
    {
        nodeDatas.ForEach(_ => _.SetValue());
        
        return controller;
    }

    /// <summary>
    /// 루트 노드 생성. BTBuilder에서 가장 먼저 호출되어야 하는 함수이다.
    /// </summary>
    public BTBuilder AddRootNode(BTRootData _data)
    {
        var node = new BTRoot();
        // Root는 트리에서 단 한번만 호출되기 때문에, 여기에서 컨트롤러를 만든다.
        controller = new BTController(node);
        node.Init(null, controller, _data);

        AddNodeAndData(node, _data);
        
        return this;
    }

    public BTBuilder AddSelectorNode(BTCaller _caller, BTSelectorData _data, int _parentId)
    {
        var parentNode = GetNode(_parentId);
        
        var node = new BTSelector();
        _caller.SetEvaluateFunction(parentNode.OnChildEvaluated, _data);
        node.Init(_caller, controller, _data);

        AddNodeAndData(node, _data);
        
        return this;
    }
    
    public BTBuilder AddSequenceNode(BTCaller _caller, BTSequenceData _data, int _parentId)
    {
        var parentNode = GetNode(_parentId);
        
        var node = new BTSequence();
        _caller.SetEvaluateFunction(parentNode.OnChildEvaluated, _data);
        node.Init(_caller, controller, _data);

        AddNodeAndData(node, _data);
        
        return this;
    }

    public BTBuilder AddIfNode(BTCaller _caller, BTIfData _data, int _parentId)
    {
        var parentNode = GetNode(_parentId);

        var node = new BTIf();
        _caller.SetEvaluateFunction(parentNode.OnChildEvaluated, _data);
        node.Init(_caller, controller, _data);
        
        AddNodeAndData(node, _data);

        return this;
    }
    
    public BTBuilder AddWhileNode(BTCaller _caller, BTWhileData _data, int _parentId)
    {
        var parentNode = GetNode(_parentId);

        var node = new BTWhile();
        _caller.SetEvaluateFunction(parentNode.OnChildEvaluated, _data);
        node.Init(_caller, controller, _data);
        
        AddNodeAndData(node, _data);

        return this;
    }
    
    public BTBuilder AddExecuteNode(BTCaller _caller, BTExecuteNodeBase _node, BTExecuteData _data, int _parentId)
    {
        var parentNode = GetNode(_parentId);
        
        _caller.SetEvaluateFunction(parentNode.OnChildEvaluated, _data);
        _node.Init(_caller, controller, _data);
        
        AddNodeAndData(_node, _data);

        return this;
    }

    private void AddNodeAndData(BTNodeBase _node, BTData _data)
    {
        nodeBases.Add(_node);
        nodeDatas.Add(_data);
    }
    
    public BTNodeBase GetNode(int _id)
    {
        return nodeBases.Find(_ => _.Data.ID == _id);
    }
    
    public System.Func<bool> GetFunction(Define.BehaviourTree.BTConditional _btConditionalFunc)
    {
        return controller.GetConditionalFunc(_btConditionalFunc);
    }
}
