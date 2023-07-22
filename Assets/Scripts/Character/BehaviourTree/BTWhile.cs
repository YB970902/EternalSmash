using System.Collections;
using System.Collections.Generic;
using Define;
using Unity.VisualScripting;
using UnityEngine;

public class BTWhile : BTControlNodeBase
{
    private int currentRepeatCount;
    protected override void Init(BTData _data)
    {
        if (_data is not BTWhileData)
        {
            Debug.LogError("data type is not BTWhileData");
            return;
        }

        Data = _data;

        currentRepeatCount = 0;
    }

    public override void Evaluate()
    {
        (Data as BTWhileData).Child.Evaluate();
    }

    public override void OnChildEvaluated(BehaviourTree.BTState _state)
    {
        base.OnChildEvaluated(_state);

        var data = Data as BTWhileData;
        
        switch (_state)
        {
            case BehaviourTree.BTState.Success:
                if (data.RepeatCount == 0) break;
                
                ++currentRepeatCount;
                if (currentRepeatCount >= data.RepeatCount) // 반복횟수를 다 채운 경우
                {
                    currentRepeatCount = 0;
                    btCaller.OnChildEvaluated(BehaviourTree.BTState.Success); // 성공 반환
                }
                break;
            case BehaviourTree.BTState.Fail:
                currentRepeatCount = 0;
                btCaller.OnChildEvaluated(BehaviourTree.BTState.Fail);
                break;
        }
    }
}
