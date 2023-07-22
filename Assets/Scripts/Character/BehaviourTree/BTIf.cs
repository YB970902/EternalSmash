using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define.BehaviourTree;

/// <summary>
/// 조건식이 맞으면 True노드를 수행하고 맞지 않으면 False노드를 수행한 후, 결과를 반환한다.
/// </summary>
public class BTIf : BTControlNodeBase
{
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

        btCaller.OnChildEvaluated(_state);
    }
}
