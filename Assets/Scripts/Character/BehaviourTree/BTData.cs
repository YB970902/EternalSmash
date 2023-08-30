using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 노드가 가지고 있어야하는 부모/자식의 ID와 그 외의 정보를 보관하고 있는 클래스이다.
/// BTData의 타입에 맞는 빈 노드를 생성하고 반환할 수 있다.
/// BTBuilder가 빈 노드를 모두 받은다음, BTData가 가지고 있는 정보를 기반기반으로 빈 노드들에 값을 채운다.
/// 데이터 가공이 끝나면 BTBuilder가 가지고 있는 빈 노드에 BTData를 넣어서 값을 채운다.
/// </summary>
public abstract class BTData
{
    /// <summary> 유효하지 않은 아이디 </summary>
    public const int InvalidID = 0;
    
    /// <summary> 부모 노드의 아이디 </summary>
    public int ParentID { get; protected set; }
    /// <summary> 노드마다 부여되는 고유한 아이디. </summary>
    public int ID { get; protected set; }

    public abstract BTNodeBase GetNodeInstance();
}

public class BTRootData : BTData
{
    public int ChildID { get; private set; }
    
    public BTRootData(int _id, int _childId)
    {
        // Root노드는 부모가 없다.
        ParentID = InvalidID;
        ID = _id;
        ChildID = _childId;
    }
    
    public override BTNodeBase GetNodeInstance()
    {
        return new BTRoot();
    }
}

public class BTExecuteData : BTData
{
    public Define.BehaviourTree.BTExecute ExecuteType { get; private set; }
    public BTExecuteData(int _id, int _parentId, Define.BehaviourTree.BTExecute _type)
    {
        ID = _id;
        ParentID = _parentId;
        ExecuteType = _type;
    }

    public override BTNodeBase GetNodeInstance()
    {
        return new BTExecuteNode();
    }
}

public class BTSelectorData : BTData
{
    public List<int> ChildrenID { get; private set; }
    public BTSelectorData(int _id, int _parentId, List<int> _childrenId)
    {
        ID = _id;
        ParentID = _parentId;
        ChildrenID = _childrenId;
    }

    public override BTNodeBase GetNodeInstance()
    {
        return new BTSelector();
    }
}

public class BTSequenceData : BTData
{
    public List<int> ChildrenID { get; private set; }
    public BTSequenceData(int _id, int _parentId, List<int> _childrenId)
    {
        ID = _id;
        ParentID = _parentId;
        ChildrenID = _childrenId;
    }

    public override BTNodeBase GetNodeInstance()
    {
        return new BTSequence();
    }
}

public class BTIfData : BTData
{
    public int TrueNodeID { get; private set; }
    public int FalseNodeID { get; private set; }
    public Define.BehaviourTree.BTConditional ConditionalFuncType { get; private set; }

    public BTIfData(int _id, int _parentId, int _trueNodeId, int _falseNodeId, Define.BehaviourTree.BTConditional _btConditionalFuncType)
    {
        ID = _id;
        ParentID = _parentId;
        TrueNodeID = _trueNodeId;
        FalseNodeID = _falseNodeId;
        ConditionalFuncType = _btConditionalFuncType;
    }

    public override BTNodeBase GetNodeInstance()
    {
        return new BTIf();
    }
}

public class BTWhileData : BTData
{
    /// <summary> 반복 횟수. 0이면 무한반복이다. </summary>
    public int RepeatCount { get; private set; }
    
    public int ChildID { get; private set; }

    public BTWhileData(int _id, int _parentId, int _childId, int _repeatCount)
    {
        ID = _id;
        ParentID = _parentId;
        ChildID = _childId;
        RepeatCount = _repeatCount;
    }

    public override BTNodeBase GetNodeInstance()
    {
        return new BTWhile();
    }
}