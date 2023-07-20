using System;
using System.Collections;
using System.Collections.Generic;
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
        var btBuilder = new BTBuilder();
        btController = btBuilder.AddRootNode(BTRootData.Create(btBuilder, 0, 1))
            .AddSequenceNode(BTSequenceData.Create(btBuilder, 1, new List<int>() { 2, 3 }), 0)
            .AddSequenceNode(BTSequenceData.Create(btBuilder, 2, new List<int>(){4, 5}), 1)
            .AddExecuteNode(new BTSetRandomTargetIndex(), BTExecuteData.Create(4), 2)
            .AddExecuteNode(new BTFindPathRandomTarget(), BTExecuteData.Create(5), 2)
            .AddWhileNode(BTWhileData.Create(btBuilder, 3, 6, 0), 1)
            .AddExecuteNode(new BTMoveToTarget(), BTExecuteData.Create(6), 3)
            .Build();
        
        btController.Init(gameObject, 0);
    }

    private void FixedUpdate()
    {
        btController.Execute();
    }
}
