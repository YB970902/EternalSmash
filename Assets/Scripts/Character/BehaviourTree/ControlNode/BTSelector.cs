using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define.BehaviourTree;

/// <summary>
/// 자식 노드중 하나가 Success를 반환하면 즉시 부모 노드에 Success를 반환하는 노드.
/// </summary>
public class BTSelector : BTControlNodeBase
{
    private List<BTNodeBase> children;
    protected override void OnInit(BTBuilder _builder, BTData _data)
    {
        var data = _data as BTSelectorData;
        if (data != null)
        {
            Debug.LogError("data type is not BTSelectorData");
            return;
        }

        children = new List<BTNodeBase>(data.ChildrenID.Count);
        foreach (var childId in data.ChildrenID)
        {
            children.Add(_builder.GetNode(childId));
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();
        currentNodeIndex = 0;
        currentNode = children[currentNodeIndex];
        currentNode.OnEnter();
        
        if (currentNode.IsExecuteNode)
        {
            isEnterExecuteNode = true;
            btController.SetRunningNode(this);
        }
    }

    public override void Evaluate()
    {
        if (isEnterExecuteNode)
        {
            isEnterExecuteNode = false;
            return;
        }
        currentNode.Evaluate();
    }

    public override void OnChildEvaluated(BTState _state)
    {
        switch (_state)
        {
            case BTState.Success:
                currentNode.OnExit();
                currentNodeIndex = 0;
                btCaller.OnChildEvaluated(BTState.Success);
                break;
            case BTState.Fail:
                currentNode.OnExit();
                ++currentNodeIndex;
                if (currentNodeIndex >= children.Count)
                {
                    // 모든 자식을 다 탐색했으나, 모든 노드가 Fail을 반환했다면, 부모에게 결과를 반환한다.
                    btCaller.OnChildEvaluated(BTState.Fail);
                    return;
                }

                currentNode = children[currentNodeIndex];
                currentNode.OnEnter();

                if (currentNode.IsExecuteNode)
                {
                    isEnterExecuteNode = true;
                    btController.SetRunningNode(this);
                }
                break;
        }
    }
}
