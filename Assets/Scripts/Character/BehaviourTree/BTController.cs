using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 행동트리의 메인이 되는 컨트롤러.
/// </summary>
public class BTController
{
    /// <summary> 현재 동작중인 노드 </summary>
    private BTControlNodeBase runningNode;

    public void SetRunningNode(BTControlNodeBase _runningNode)
    {
        if (runningNode != _runningNode)
        {
            // 동작중인 노드가 바뀌었다면, 기존 노드는 동작노드가 아니게 된다.
            runningNode?.SetRunningEnd();
        }

        runningNode = _runningNode;
    }
}