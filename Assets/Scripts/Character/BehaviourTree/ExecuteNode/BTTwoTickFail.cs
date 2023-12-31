using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTTwoTickFail : BTExecuteNodeBase
{
    private int tickCount = 0;
    
    protected override void Init()
    {
        
    }
    
    public override void OnEnter()
    {
        base.OnEnter();
        tickCount = 0;
        Debug.Log("OnEnter");
    }

    public override void OnExit()
    {
        base.OnExit();
        Debug.Log("OnExit");
    }

    public override void Evaluate()
    {
        tickCount++;
        if (tickCount >= 2)
        {
            btCaller.OnChildEvaluated(Define.BehaviourTree.BTState.Fail);
        }
        else
        {
            btCaller.OnChildEvaluated(Define.BehaviourTree.BTState.Running);
        }
        
        Debug.Log($"NowMilliSeconds : [{DateTime.Now.Millisecond}]");
    }
}
