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
            .AddSequenceNode(btCaller.Clone(), BTSequenceData.Create(btBuilder, 1, new List<int>(3){2, 3, 4}), 0)
            .AddExecuteNode(btCaller.Clone(), new BTTwoTickSuccess(), BTExecuteData.Create(2), 1)
            .AddExecuteNode(btCaller.Clone(), new BTTwoTickSuccess(), BTExecuteData.Create(3), 1)
            .AddExecuteNode(btCaller.Clone(), new BTTwoTickSuccess(), BTExecuteData.Create(4), 1)
            .Build();
    }

    private void FixedUpdate()
    {
        btController.Execute();
    }
}
