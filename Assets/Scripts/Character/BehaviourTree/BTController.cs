using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;

using static Define.BehaviourTree;

/// <summary>
/// 행동트리의 메인이 되는 컨트롤러.
/// </summary>
public class BTController
{
    /// <summary> 루트 노드 </summary>
    private BTRoot rootNode;
    /// <summary> 현재 동작중인 노드 </summary>
    private BTControlNodeBase runningNode;
    /// <summary> 행동에 관련된 데이터 모음 </summary>
    public BTBlackBoard BlackBoard { get; private set; }

    /// <summary> 길찾기 가이드 </summary>
    public PathGuide PathGuide { get; private set; }

    public BTController(BTRoot _rootNode)
    {
        rootNode = _rootNode;
    }

    /// <summary>
    /// 생성될때 초기화하는 함수.
    /// </summary>
    public void Init(GameObject _obj, int _startIndex)
    {
        PathGuide = new PathGuide();
        PathGuide.Set(_obj.GetComponent<MovementController>(), _startIndex);

        BlackBoard = new BTBlackBoard();
    }
    
    /// <summary>
    /// 노드를 실행하는 함수
    /// </summary>
    public void Execute()
    {
        if (runningNode != null)
        {
            runningNode.Evaluate();
        }
        else
        {
            rootNode.Evaluate();
        }
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
    
    /// <summary>
    /// 현재 실행중인 노드를 설정하는 함수
    /// </summary>
    public void SetRunningNode(BTControlNodeBase _runningNode)
    {
        // 기존 노드와 같다면 반환한다.
        if (runningNode == _runningNode) return;
        
        // 동작중인 노드를 바꾸기 전에 기존 노드를 종료한다.
        runningNode?.SetRunningEnd();
        runningNode = _runningNode;
    }
    
    #region ConditionalFunction
    
    private bool True()
    {
        return true;
    }

    private bool False()
    {
        return false;
    }
    
    #endregion
}