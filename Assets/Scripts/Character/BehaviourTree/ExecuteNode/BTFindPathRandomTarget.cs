using System.Collections;
using System.Collections.Generic;
using Define;
using UnityEngine;

public class BTFindPathRandomTarget : BTExecuteNodeBase
{
    protected override void Init()
    {
        
    }

    public override void OnEnter()
    {
        base.OnEnter();
        btController.PathGuide.SetTargetIndex(btController.BlackBoard.RandomTargetNode);
    }

    public override void Evaluate()
    {
        if (btController.PathGuide.IsWaitPath == false) // 경로 탐색이 완료된경우
        {
            btCaller.OnChildEvaluated(BehaviourTree.BTState.Success);
            return;
        }
        
        btCaller.OnChildEvaluated(BehaviourTree.BTState.Running);
    }
}
