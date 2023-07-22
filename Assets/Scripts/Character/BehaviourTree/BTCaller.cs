using System;
using static Define.BehaviourTree;
using Debug = UnityEngine.Debug;

public abstract class BTCaller
{
    protected System.Action<BTState> cbOnChildEvaluated;

    protected BTData data;
    
    public void SetEvaluateFunction(System.Action<BTState> _func, BTData _data)
    {
        cbOnChildEvaluated = _func;
        data = _data;
    }
    
    /// <summary>
    /// 부모 노드에게 판정 결과를 반환하는 함수
    /// </summary>
    public abstract void OnChildEvaluated(BTState _state);
}

public class BTDefaultCaller : BTCaller
{
    public override void OnChildEvaluated(BTState _state)
    {
        cbOnChildEvaluated.Invoke(_state);
    }
}


public class BTDebugCaller : BTCaller
{
    private BTDebuggerFlag flag;
    public void Init(BTDebuggerFlag _flag)
    {
        flag = _flag;
    }
    
    public override void OnChildEvaluated(BTState _state)
    {
        cbOnChildEvaluated.Invoke(_state);
        switch (_state)
        {
            case BTState.Success:
                if((flag & BTDebuggerFlag.CheckSuccess) == 0) return;
                break; 
            case BTState.Fail:
                if((flag & BTDebuggerFlag.CheckFail) == 0) return;
                break;
            case BTState.Running:
                if((flag & BTDebuggerFlag.CheckRunning) == 0) return;
                break;
            default: return;
        }
        
        var methodInfo = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod();
        var name = methodInfo.ReflectedType?.Name ?? string.Empty;
        
        Debug.Log($"caller : [{name}] id : [{data.ID}] state : [{_state.ToString()}]");
    }
}