using System.Collections;
using System.Collections.Generic;
using Define;
using Unity.VisualScripting;
using UnityEngine;

public class BTWhile : BTControlNodeBase
{
    private int currentRepeatCount;

    private int maxRepeatCount;
    private BTNodeBase child;
    protected override void OnInit(BTBuilder _builder, BTData _data)
    {
        var data = _data as BTWhileData;
        if (data != null)
        {
            Debug.LogError("data type is not BTWhileData");
            return;
        }

        maxRepeatCount = data.RepeatCount;
        child = _builder.GetNode(data.ChildID);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        currentRepeatCount = 0;
        currentNode = child;
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
        switch (_state)
        {
            case BehaviourTree.BTState.Success:
                currentNode.OnExit();
                
                if (maxRepeatCount == 0)
                {
                    currentNode.OnEnter();
                    break;
                }
                
                ++currentRepeatCount;
                if (currentRepeatCount >= maxRepeatCount) // 반복횟수를 다 채운 경우
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
