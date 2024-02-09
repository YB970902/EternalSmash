using System;
using System.Collections;
using System.Collections.Generic;
using Define;
using UnityEngine;

public class BTTester : MonoBehaviour
{
    private BTController btController;
    
    void Start()
    {
        BTDebugCaller btCaller = new BTDebugCaller();
        btCaller.Init(BehaviourTree.BTDebuggerFlag.CheckFail | BehaviourTree.BTDebuggerFlag.CheckRunning | BehaviourTree.BTDebuggerFlag.CheckSuccess);
        var btBuilder = BTBuilder.Instance;
        btBuilder.Set(btCaller);
        btController = btBuilder.AddNodeData(new BTRootData( 0, 1))
            .AddNodeData(new BTSequenceData(1, 0, new List<int>(3){2, 3, 4}))
            .AddNodeData(new BTExecuteData(2, 1, BehaviourTree.Execute.TwoTickSuccess))
            .AddNodeData(new BTExecuteData(3, 1, BehaviourTree.Execute.TwoTickFail))
            .AddNodeData(new BTExecuteData(4, 1, BehaviourTree.Execute.TwoTickSuccess))
            .Build();
    }

    private void FixedUpdate()
    {
        btController.Execute();
    }
}
