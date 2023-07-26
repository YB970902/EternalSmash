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
    }

    public override void OnEnter()
    {
        base.OnEnter();
        currentRepeatCount = 0;
        currentNode = (Data as BTWhileData).Child;
        currentNode.OnEnter();
        
        if (currentNode.IsExecuteNode)
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
        
        currentNode.Evaluate();
    }

    public override void OnChildEvaluated(BehaviourTree.BTState _state)
    {
        var data = Data as BTWhileData;
        
        switch (_state)
        {
            case BehaviourTree.BTState.Success:
                currentNode.OnExit();
                
                if (data.RepeatCount == 0)
                {
                    currentNode.OnEnter();
                    break;
                }
                
                ++currentRepeatCount;
                if (currentRepeatCount >= data.RepeatCount) // 반복횟수를 다 채운 경우
                {
                    btCaller.OnChildEvaluated(BehaviourTree.BTState.Success); // 성공 반환
                    break;
                }
                
                currentNode.OnEnter();
                break;
            case BehaviourTree.BTState.Fail:
                btCaller.OnChildEvaluated(BehaviourTree.BTState.Fail);
                break;
        }
    }
}
