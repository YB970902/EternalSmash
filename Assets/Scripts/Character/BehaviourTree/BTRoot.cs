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
        
    }
}
