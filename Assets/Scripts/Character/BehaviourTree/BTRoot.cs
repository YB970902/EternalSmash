using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTRoot : BTNodeBase
{
    /// <summary> 자식의 OnEnter를 1회만 호출하기 위한 플래그 </summary>
    private bool isRunning;
    
    protected override void Init(BTData _data)
    {
        if (_data is not BTRootData)
        {
            Debug.LogError("data type is not BTRootData");
            return;
        }

        Data = _data;
        isRunning = false;
    }

    public override void Evaluate()
    {
        var data = (Data as BTRootData);
        if (isRunning == false)
        {
            data.Child.OnEnter();
            isRunning = true;
        }
        data.Child.Evaluate();
    }

    public override void OnChildEvaluated(Define.BehaviourTree.BTState _state)
    {
        isRunning = false;
        (Data as BTRootData).Child.OnExit();
        // Root노드로 돌아왔다면 Running노드를 제거한다.
        btController.SetRunningNode(null);
    }
}
