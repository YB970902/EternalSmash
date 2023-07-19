using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Define.BehaviourTree;

/// <summary>
/// 행동트리의 메인이 되는 컨트롤러.
/// </summary>
public class BTController
{
    /// <summary> 현재 동작중인 노드 </summary>
    private BTControlNodeBase runningNode;

    private BTRoot rootNode;

    public BTController(BTRoot _rootNode)
    {
        rootNode = _rootNode;
    }

    /// <summary>
    /// 함수의 이름을 받으면 그 이름에 맞는 함수를 반환한다.
    /// </summary>
    public System.Func<bool> GetConditionalFunc(string _funcName)
    {
        switch (_funcName)
        {
            case Conditional.True: return True;
            case Conditional.False: return False;
        }

        return null;
    }

    private bool True()
    {
        return true;
    }

    private bool False()
    {
        return false;
    }
    
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