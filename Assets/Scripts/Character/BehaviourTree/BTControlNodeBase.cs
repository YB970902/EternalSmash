using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 조건에 따라 하위노드중 어떤 노드를 평가할지 선택하는 노드의 베이스
/// </summary>
public abstract class BTControlNodeBase : BTNodeBase
{
    /// <summary> 현재 동작중인 노드의 인덱스 </summary>
    protected int curNodeIndex;
    
    /// <summary> 러닝 노드인지 여부 </summary>
    protected bool isRunning;

    /// <summary>
    /// 러닝 노드 상태가 종료됨
    /// </summary>
    public void SetRunningEnd()
    {
        isRunning = false;
        curNodeIndex = 0;
    }
    
    public override void OnChildEvaluated(Define.BehaviourTree.BTState _state)
    {
        if (_state != Define.BehaviourTree.BTState.Running || isRunning != false) return;
        
        //현재 평가중인 자식이 아직 동작중이라면, 자신은 RunningNode가 된다.
        //또, 부모에게 결과를 반환하지 않는다.
        btController.SetRunningNode(this);
        isRunning = true;
    }
}
