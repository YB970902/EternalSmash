using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTMoveToTarget : BTExecuteNodeBase
{
    protected override void Init()
    {
        
    }
    
    public override void Evaluate()
    {
        btCaller.OnChildEvaluated(btController.PathGuide.Tick());
    }
}
