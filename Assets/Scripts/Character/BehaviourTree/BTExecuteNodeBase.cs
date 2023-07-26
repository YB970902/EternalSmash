using Define;
using UnityEngine;

/// <summary>
/// 행동 노드의 기본 클래스
/// </summary>
public abstract class BTExecuteNodeBase : BTNodeBase
{
    protected override void Init(BTData _data)
    {
        if (_data is not BTExecuteData)
        {
            Debug.LogError("Data type is not BTExecuteData");
            return;
        }
        
        Data = _data;
        IsExecuteNode = true;
        
        Init();
    }
    
    protected abstract void Init();

    /// <summary>
    /// 행동 노드는 자식이 있을 수 없으므로 강제로 구현할 필요가 없게한다.
    /// 없앨 수 있다면 없애는게 낫겠다.
    /// </summary>
    public override void OnChildEvaluated(BehaviourTree.BTState _state)
    { 
        
    }
}
