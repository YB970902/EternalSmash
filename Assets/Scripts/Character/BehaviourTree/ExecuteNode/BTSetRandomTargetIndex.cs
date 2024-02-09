using System.Collections;
using System.Collections.Generic;
using Define;
using UnityEngine;

/// <summary>
/// 랜덤한 위치의 목표 위치를 잡는 행동 노드
/// </summary>
public class BTSetRandomTargetIndex : BTExecuteAction
{
    protected override void OnInit()
    {
        
    }

    public override void OnEnter()
    {
        
    }

    public override void OnExit()
    {
        
    }

    public override BehaviourTree.BTState Evaluate()
    {
        // 랜덤한 위치로 지정한다.
        btController.BlackBoard.RandomTargetNode = Random.Range(0, TileModule.TotalCount);
        return BehaviourTree.BTState.Success;
    }
}
