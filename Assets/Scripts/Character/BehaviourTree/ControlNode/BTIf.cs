using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define.BehaviourTree;

/// <summary>
/// 조건식이 맞으면 True노드를 수행하고 맞지 않으면 False노드를 수행한 후, 결과를 반환한다.
/// 해당 노드가 비어있다면, Fail을 반환한다.
/// </summary>
public class BTIf : BTControlNodeBase
{
    /// <summary> 조건 함수의 결과가 참인지 거짓인지 여부 </summary>
    private bool isTrue;
    protected override void Init(BTData _data)
    {
        if (_data is not BTIfData)
        {
            Debug.LogError("data type is not BTIfData");
            return;
        }

        Data = _data;
    }

    public override void OnEnter()
    {
        Debug.Log("OnIfEnter");
        base.OnEnter();
        isTrue = (Data as BTIfData).ConditionalFunc.Invoke();
        var data = Data as BTIfData;
        if (isTrue)
        {
            data.TrueNode?.OnEnter();
        }
        else
        {
            data.FalseNode?.OnEnter();
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        Debug.Log("OnIfExit");
    }

    public override void Evaluate()
    {
        var data = Data as BTIfData;
        if (isTrue && data.TrueNode != null)
        {
            data.TrueNode.Evaluate();
        }
        else if (isTrue == false && data.FalseNode != null)
        {
            data.FalseNode.Evaluate();
        }
        else
        {
            // 알맞는 노드가 비어있다면 부모 노드에게 Fail을 반환한다.
            btCaller.OnChildEvaluated(BTState.Fail);
        }
    }

    public override void OnChildEvaluated(BTState _state)
    {
        base.OnChildEvaluated(_state);

        if (_state == BTState.Running) return;

        var data = Data as BTIfData;
        
        if (isTrue)
        {
            data.TrueNode?.OnExit();            
        }
        else
        {
            data.FalseNode?.OnExit();
        }
        
        btCaller.OnChildEvaluated(_state);
    }
}
