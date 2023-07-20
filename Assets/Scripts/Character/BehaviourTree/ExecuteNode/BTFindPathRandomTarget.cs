using System.Collections;
using System.Collections.Generic;
using Define;
using UnityEngine;

public class BTFindPathRandomTarget : BTExecuteNodeBase
{
    protected override void Init()
    {
        
    }
    
    public override void Evaluate()
    {
        var pathGuide = btController.PathGuide;
        
        if (pathGuide.IsReadyToMove) // 경로 탐색이 완료된경우
        {
            pathGuide.SetMoveStart();
            cbEvaluate(BehaviourTree.BTState.Success);
            return;
        }
        
        if (pathGuide.IsWaitPath == false) // 아직 길찾기 시작을 안한경우
        {
            pathGuide.SetTargetIndex(btController.BlackBoard.RandomTargetNode);
        }
        
        cbEvaluate(BehaviourTree.BTState.Running);
    }
}
