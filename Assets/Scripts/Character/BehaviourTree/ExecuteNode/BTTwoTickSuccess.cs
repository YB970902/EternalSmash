using Define;

public class BTTwoTickSuccess : BTExecuteAction
{
    private int tickCount = 0;

    protected override void OnInit()
    {
        
    }

    public override void OnEnter()
    {
        tickCount = 0;
    }

    public override void OnExit()
    {
        
    }

    public override BehaviourTree.BTState Evaluate()
    {
        tickCount++;
        if (tickCount >= 2)
        {
            return BehaviourTree.BTState.Success;
        }
        
        return Define.BehaviourTree.BTState.Running;
    }
}
