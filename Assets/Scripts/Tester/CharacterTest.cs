using System;
using System.Collections;
using System.Collections.Generic;
using Define;
using UnityEngine;

public class CharacterTest : MonoBehaviour
{
    private BTController btController;
    private void Start()
    {
        // 0 : 루트
        // 1 : 시퀀스
        // 2 : 시퀀스              3 : 리핏
        // 4 : 세팅, 5: 탐색        6 : 위치 이동
        var btCaller = new BTDebugCaller();
        btCaller.Init(BehaviourTree.BTDebuggerFlag.CheckSuccess | BehaviourTree.BTDebuggerFlag.CheckFail | BehaviourTree.BTDebuggerFlag.CheckRunning);
        var btBuilder = BTBuilder.Instance;
        btBuilder.Set(btCaller);
        
        btController = btBuilder.AddNodeData(new BTRootData(0, 1))
            .AddNodeData(new BTSequenceData(1, 0, new List<int>() { 2, 3 }))
            .AddNodeData(new BTSequenceData(2, 1, new List<int>(){4, 5}))
            .AddNodeData(new BTExecuteData(4, 2, BehaviourTree.BTExecute.BTSetRandomTargetIndex))
            .AddNodeData(new BTExecuteData(5, 2, BehaviourTree.BTExecute.BTFindPathRandomTarget))
            .AddNodeData(new BTWhileData(3, 1, 6, 0))
            .AddNodeData(new BTExecuteData(6, 3, BehaviourTree.BTExecute.BTMoveToTarget))
            .Build();
        
        btController.Init(gameObject, 0);
    }

    private void FixedUpdate()
    {
        btController.Execute();
    }
}
