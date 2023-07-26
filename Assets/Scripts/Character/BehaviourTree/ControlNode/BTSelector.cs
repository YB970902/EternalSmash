using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define.BehaviourTree;

/// <summary>
/// 자식 노드중 하나가 Success를 반환하면 즉시 부모 노드에 Success를 반환하는 노드.
/// </summary>
public class BTSelector : BTControlNodeBase
{
    protected override void Init(BTData _data)
    {
        if (_data is not BTSelectorData)
        {
            Debug.LogError("data type is not BTSelectorData");
            return;
        }

        Data = _data;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("OnSelectorEnter");
        (Data as BTSelectorData).Children[curNodeIndex].OnEnter();
    }

    public override void OnExit()
    {
        base.OnExit();
        Debug.Log("OnSelectorExit");
    }

    public override void Evaluate()
    {
        (Data as BTSelectorData).Children[curNodeIndex].Evaluate();
    }

    public override void OnChildEvaluated(BTState _state)
    {
        base.OnChildEvaluated(_state);

        var data = Data as BTSelectorData;
        
        if (_state == BTState.Success)
        {
            data.Children[curNodeIndex].OnExit();
            curNodeIndex = 0;
            btCaller.OnChildEvaluated(BTState.Success);
        }
        else if (_state == BTState.Fail)
        {
            data.Children[curNodeIndex].OnExit();
            ++curNodeIndex;
            if (curNodeIndex >= (Data as BTSelectorData).Children.Count)
            {
                // 모든 자식을 다 탐색했으나, 모든 노드가 Fail을 반환했다면, 부모에게 결과를 반환한다.
                curNodeIndex = 0;
                btCaller.OnChildEvaluated(BTState.Fail);
                return;
            }
            
            if (isRunning == false)
            {
                // 노드가 옮겨질때마다 저장한다.
                btController.SetRunningNode(this);
            }
            data.Children[curNodeIndex].OnEnter();
        }
    }
}
