using Define;

public class BTMoveToTarget : BTExecuteAction
{
    protected override void OnInit()
    {

    }

    public override void OnEnter()
    {
        
    }

    public override void OnExit()
    {
        
    }

    public override BehaviourTree.BTState Evaluate()
    {
        return btController.PathGuide.Tick();
    }
}