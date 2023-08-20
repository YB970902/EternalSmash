using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 행동트리의 노드가 기본적으로 가져올 값이다.
/// 각 노드별로 필요한 데이터들은 이 클래스로부 파생되어야 한다.
/// 만약 BTNodeBase를 변수로 가지고 있어야 한다면, 노드의 Id를 따로 가지고 있어야 한다. 
/// </summary>
public class BTData
{
    /// <summary> 노드의 아이디가 유효하지 않은지 구분하기 위한 값. </summary>
    public const int InvalidID = 0;
    
    /// <summary> 노드마다 부여되는 고유한 아이디. 1부터 시작한다. </summary>
    public int ID { get; protected set; }

    /// <summary> 노드를 값을 받아내는 함수 </summary>
    protected System.Action nodeSetSequence;

    /// <summary>
    /// 받아야하는 노드의 ID만 보관중이던 액션을 수행하는 함수.
    /// </summary>
    public void SetValue()
    {
        nodeSetSequence?.Invoke();
        nodeSetSequence = null;
    }
}

public class BTRootData : BTData
{
    public BTNodeBase Child { get; private set; }

    public static BTRootData Create(BTBuilder _builder, int _id, int _childId)
    {
        var result = new BTRootData();
        result.ID = _id;
        result.nodeSetSequence += () => { result.Child = _builder.GetNode(_childId); };
        return result;
    }
}

public class BTExecuteData : BTData
{
    public static BTExecuteData Create(int _id)
    {
        var result = new BTExecuteData();
        result.ID = _id;
        return result;
    }
}

public class BTSelectorData : BTData
{
    public List<BTNodeBase> Children { get; private set; }
    public static BTSelectorData Create(BTBuilder _builder, int _id, List<int> _childrenId)
    {
        var result = new BTSelectorData();
        result.ID = _id;
        result.Children = new List<BTNodeBase>(_childrenId.Count);
        for (int i = 0, count = _childrenId.Count; i < count; ++i)
        {
            result.Children.Add(null);
            var index = i;
            var nodeId = _childrenId[i];
            result.nodeSetSequence += () => { result.Children[index] = _builder.GetNode(nodeId); };
        }
        return result;
    }
}

public class BTSequenceData : BTData
{
    public List<BTNodeBase> Children { get; private set; }
    public static BTSequenceData Create(BTBuilder _builder, int _id, List<int> _childrenId)
    {
        var result = new BTSequenceData();
        result.ID = _id;
        result.Children = new List<BTNodeBase>(_childrenId.Count);
        for (int i = 0, count = _childrenId.Count; i < count; ++i)
        {
            result.Children.Add(null);
            var index = i;
            var nodeId = _childrenId[i];
            result.nodeSetSequence += () => { result.Children[index] = _builder.GetNode(nodeId); };
        }
        return result;
    }
}

public class BTIfData : BTData
{
    public BTNodeBase TrueNode { get; private set; }
    public BTNodeBase FalseNode { get; private set; }
    
    public System.Func<bool> ConditionalFunc { get; private set; }

    public static BTIfData Create(BTBuilder _builder, int _id, int _trueNodeId, int _falseNodeId, Define.BehaviourTree.BTConditional _btConditionalFunc)
    {
        var result = new BTIfData();
        result.ID = _id;
        result.nodeSetSequence += () => { result.TrueNode = _builder.GetNode(_trueNodeId); };
        result.nodeSetSequence += () => { result.FalseNode = _builder.GetNode(_falseNodeId); };
        result.ConditionalFunc = _builder.GetFunction(_btConditionalFunc);
        return result;
    }
}

public class BTWhileData : BTData
{
    /// <summary> 반복 횟수. 0이면 무한반복이다. </summary>
    public int RepeatCount { get; private set; }
    
    public BTNodeBase Child { get; private set; }

    public static BTWhileData Create(BTBuilder _builder, int _id, int _childId, int _repeatCount)
    {
        var result = new BTWhileData();
        result.ID = _id;
        result.nodeSetSequence += () => { result.Child = _builder.GetNode(_childId); };
        result.RepeatCount = _repeatCount;
        return result;
    }
}