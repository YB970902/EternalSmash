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
        BTBuilder btBuilder = new BTBuilder();
        BTDebugCaller btCaller = new BTDebugCaller();
        btCaller.Init(BehaviourTree.BTDebuggerFlag.CheckFail | BehaviourTree.BTDebuggerFlag.CheckRunning | BehaviourTree.BTDebuggerFlag.CheckSuccess);
        btController = btBuilder.AddRootNode(BTRootData.Create(btBuilder, 0, 1))
            .AddWhileNode(btCaller.Clone(), BTWhileData.Create(btBuilder, 1, 2, 5), 0)
            .AddExecuteNode(btCaller.Clone(), new BTTwoTickSuccess(), BTExecuteData.Create(2), 1)
            .Build();
    }

    private void FixedUpdate()
    {
        btController.Execute();
    }
}
