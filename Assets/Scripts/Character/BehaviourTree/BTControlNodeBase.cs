using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 조건에 따라 하위노드중 어떤 노드를 평가할지 선택하는 노드의 베이스
/// </summary>
public abstract class BTControlNodeBase : BTNodeBase
{
    /// <summary> 현재 동작중인 노드의 인덱스 </summary>
    protected int currentNodeIndex;

    /// <summary> 현재 동작중인 노드 </summary>
    protected BTNodeBase currentNode;

    /// <summary>
    /// Execute노드의 Enter가 호출된 직후인지 여부
    /// Execute노드는 한 Tick당 한번 Evaluate되어야 하므로 체크한다.
    /// </summary>
    protected bool isEnterExecuteNode;

    protected override void OnInit(BTBuilder _builder, BTData _data)
    {
        isEnterExecuteNode = false;
    }

    public override void OnRunningEnd()
    {
        base.OnRunningEnd();
        OnExit();
    }
}
