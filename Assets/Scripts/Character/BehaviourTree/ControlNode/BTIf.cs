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

    private BTNodeBase trueNode;
    private BTNodeBase falseNode;
    private System.Func<bool> conditionalFunc;

    protected override void OnInit(BTBuilder _builder, BTData _data)
    {
        var data = _data as BTIfData;
        if (data != null)
        {
            Debug.LogError("data type is not BTIfData");
            return;
        }

        trueNode = _builder.GetNode(data.TrueNodeID);
        falseNode = _builder.GetNode(data.FalseNodeID);
        conditionalFunc = btController.GetConditionalFunc(data.ConditionalFuncType);
        currentNode = null;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        
        isTrue = conditionalFunc.Invoke();
        currentNode = isTrue ? trueNode : falseNode;
        currentNode?.OnEnter();
        
        if (currentNode?.IsExecuteNode ?? false)
        {
            isEnterExecuteNode = true;
            btController.SetRunningNode(this);
        }
    }

    public override void Evaluate()
    {
        if (isEnterExecuteNode)
        {
            isEnterExecuteNode = false;
            return;
        }
        
        if (currentNode != null)
        {
            currentNode.Evaluate();
        }
        else
        {
            // 알맞는 노드가 비어있다면 부모 노드에게 Fail을 반환한다.
            btCaller.OnChildEvaluated(BTState.Fail);
        }
    }

    public override void OnChildEvaluated(BTState _state)
    {
        if (_state == BTState.Running) return;

        currentNode?.OnExit();
        
        btCaller.OnChildEvaluated(_state);
    }
}