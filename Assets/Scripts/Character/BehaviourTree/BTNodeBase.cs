using static Define.BehaviourTree;

/// <summary>
/// 행동트리 노드의 기본이 되는 클래스
/// </summary>
public abstract class BTNodeBase
{
    public BTData Data { get; protected set; }

    protected BTController btController;

    protected BTCaller btCaller;

    /// <summary>
    /// 생성되고 나서 1회 호출되는 함수로, 부모 노드에게 결과를 반환하는 함수를 받는다.
    /// </summary>
    public void Init(BTCaller _btCaller, BTController _btController, BTData _data)
    {
        btCaller = _btCaller;
        btController = _btController;
        Init(_data);
    }

    /// <summary>
    /// 데이터를 세팅하는 부분.
    /// </summary>
    protected abstract void Init(BTData _data);

    /// <summary>
    /// 평가하는 함수.
    /// </summary>
    public abstract void Evaluate();

    /// <summary>
    /// 자식의 평가가 끝난경우 결과를 반환받는 함수.
    /// </summary>
    public abstract void OnChildEvaluated(BTState _state);
}
