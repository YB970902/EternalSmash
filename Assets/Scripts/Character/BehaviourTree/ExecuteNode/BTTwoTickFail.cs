using System;
using System.Collections;
using System.Collections.Generic;
using Define;
using UnityEngine;

public class BTTwoTickFail : BTExecuteAction
{
    private int tickCount = 0;

    protected override void OnInit()
    {
        
    }

    public override void OnEnter()
    {
        tickCount = 0;
    }

    public override void OnExit()
    {
        
    }

    public override BehaviourTree.BTState Evaluate()
    {
        tickCount++;
        if (tickCount >= 2)
        {
            return Define.BehaviourTree.BTState.Fail;
        }
        
        return Define.BehaviourTree.BTState.Running;
    }
}
