using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTRoot : BTNodeBase
{
    protected override void Init(BTData _data)
    {
        if (_data is not BTRootData)
        {
            Debug.LogError("data type is not BTRootData");
            return;
        }

        Data = _data;
    }

    public override void Evaluate()
    {
        (Data as BTRootData).Child.Evaluate();
    }

    public override void OnChildEvaluated(Define.BehaviourTree.BTState _state)
    {
        // 다시 자식 노드를 수행시킨다.
        Evaluate();
    }
}
