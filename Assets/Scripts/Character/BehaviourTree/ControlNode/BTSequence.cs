using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Define.BehaviourTree;

/// <summary>
/// 자식 노드중 하나가 Fail을 반환하면 즉시 부모 노드에 Fail을 반환하는 노드.
/// </summary>
public class BTSequence : BTControlNodeBase
{
    protected override void Init(BTData _data)
    {
        if (_data is not BTSequenceData)
        {
            Debug.LogError("data type is not BTSequenceData");
            return;
        }

        Data = _data;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        currentNodeIndex = 0;
        currentNode = (Data as BTSequenceData).Children[currentNodeIndex];
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

    public override void OnChildEvaluated(BTState _state)
    {
        var data = Data as BTSequenceData;

        switch (_state)
        {
            case BTState.Fail:
                currentNode.OnExit();
                currentNodeIndex = 0;
                btCaller.OnChildEvaluated(BTState.Fail);
                break;
            case BTState.Success:
                currentNode.OnExit();
                ++currentNodeIndex;
                if (currentNodeIndex >= data.Children.Count)
                {
                    // 모든 자식을 다 탐색했으나, 모든 노드가 Success를 반환했다면, 부모에게 결과를 반환한다.
                    btCaller.OnChildEvaluated(BTState.Success);
                    return;
                }

                currentNode = data.Children[currentNodeIndex];
                currentNode.OnEnter();

                if (currentNode.IsExecuteNode)
                {
                    isEnterExecuteNode = true;
                    btController.SetRunningNode(this);
                }
                break;
        }
    }
}
