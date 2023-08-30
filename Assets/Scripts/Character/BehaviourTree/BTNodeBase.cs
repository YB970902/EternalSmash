using static Define.BehaviourTree;

/// <summary>
/// 행동트리 노드의 기본이 되는 클래스
/// </summary>
public abstract class BTNodeBase
{
    /// <summary> 노드 고유의 아이디 </summary>
    public int ID { get; protected set; }
    
    /// <summary> 말단 노드인지 체크하기 위한 플래그 </summary>
    public bool IsExecuteNode { get; protected set; }

    protected BTController btController;

    protected BTCaller btCaller;

    /// <summary>
    /// 생성되고 나서 1회 호출되는 함수로, 부모 노드에게 결과를 반환하는 함수를 받는다.
    /// </summary>
    public void Init(BTBuilder _btBuilder, BTData _data)
    {
        IsExecuteNode = false;
        ID = _data.ID;
        btCaller = _btBuilder.Caller;
        btCaller.SetEvaluateFunction(OnChildEvaluated, _data);
        btController = _btBuilder.Controller;
        
        OnInit(_btBuilder, _data);
    }

    /// <summary>
    /// 데이터를 세팅하는 부분.
    /// </summary>
    protected abstract void OnInit(BTBuilder _btBuilder, BTData _data);

    /// <summary>
    /// 평가하는 함수.
    /// </summary>
    public abstract void Evaluate();

    /// <summary>
    /// 첫 Evaluate가 호출되기 전에 호출되는 함수
    /// </summary>
    public virtual void OnEnter()
    {
        
    }

    /// <summary>
    /// 마지막 Evaluate가 호출된 후에 호출되는 함수 
    /// </summary>
    public virtual void OnExit()
    {
        
    }

    /// <summary>
    /// 진행중인 노드가 도중에 중단될때 호출되는 함수
    /// </summary>
    public virtual void OnRunningEnd()
    {
        
    }

    /// <summary>
    /// 자식의 평가가 끝난경우 결과를 반환받는 함수.
    /// </summary>
    public abstract void OnChildEvaluated(BTState _state);
}
