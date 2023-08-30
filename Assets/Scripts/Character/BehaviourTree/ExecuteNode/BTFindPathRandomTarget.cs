using System.Collections;
using System.Collections.Generic;
using Define;
using UnityEngine;

public class BTFindPathRandomTarget : BTExecuteAction
{
    protected override void OnInit()
    {

    }
    
    public override void OnEnter()
    {
        btController.PathGuide.SetTargetIndex(btController.BlackBoard.RandomTargetNode);
    }

    public override void OnExit()
    {
        
    }
    
    public override BehaviourTree.BTState Evaluate()
    {
        if (btController.PathGuide.IsWaitPath == false) // 경로 탐색이 완료된경우
        {
            return BehaviourTree.BTState.Success;
        }

        return BehaviourTree.BTState.Running;
    }
}
