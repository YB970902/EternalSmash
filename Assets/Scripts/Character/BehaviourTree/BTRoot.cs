using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTRoot : BTNodeBase
{
    /// <summary> 자식의 OnEnter를 1회만 호출하기 위한 플래그 </summary>
    private bool isRunning;

    private BTNodeBase child;
    
    protected override void OnInit(BTBuilder _builder, BTData _data)
    {
        var data = _data as BTRootData;
        if (data == null)
        {
            Debug.LogError("data type is not BTRootData");
            return;
        }

        child = _builder.GetNode(data.ChildID);
        isRunning = false;
    }

    public override void Evaluate()
    {
        if (isRunning == false)
        {
            child.OnEnter();
            isRunning = true;
        }
        child.Evaluate();
    }

    public override void OnChildEvaluated(Define.BehaviourTree.BTState _state)
    {
        isRunning = false;
        child.OnExit();
        // Root노드로 돌아왔다면 Running노드를 제거한다.
        btController.SetRunningNode(null);
    }
}
