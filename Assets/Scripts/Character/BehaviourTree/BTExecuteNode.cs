using Define;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 행동 노드의 기본 클래스
/// </summary>
public class BTExecuteNode : BTNodeBase
{
    private BTExecuteAction btExecuteAction;
    
    protected override void OnInit(BTBuilder _builder, BTData _data)
    {
        var data = _data as BTExecuteData;
        if (data != null)
        {
            Debug.LogError("Data type is not BTExecuteData");
            return;
        }

        btExecuteAction = btController.GetExecuteAction(data.ExecuteType);
        IsExecuteNode = true;
        
        // TODO : executeAction에 값을 넣어준다.
        
        btExecuteAction.Init(btController);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        btExecuteAction.OnEnter();
    }

    public override void OnExit()
    {
        base.OnExit();
        btExecuteAction.OnExit();
    }

    /// <summary>
    /// 행동 노드는 자식이 있을 수 없으므로 강제로 구현할 필요가 없게한다.
    /// 없앨 수 있다면 없애는게 낫겠다.
    /// </summary>
    public override void OnChildEvaluated(BehaviourTree.BTState _state)
    {
        
    }

    public override void Evaluate()
    {
        btCaller.OnChildEvaluated(btExecuteAction.Evaluate());
    }
}

public abstract class BTExecuteAction
{
    protected BTController btController;

    public void Init(BTController _btController)
    {
        btController = _btController;
        OnInit();
    }

    /// <summary>
    /// 초기화되는 순간 호출되는 함수
    /// </summary>
    protected abstract void OnInit();

    public abstract void OnEnter();

    public abstract void OnExit();

    public abstract BehaviourTree.BTState Evaluate();
}
