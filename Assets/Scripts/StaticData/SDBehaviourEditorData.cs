using System.Collections.Generic;
using MemoryPack;
using Define;
using UnityEngine;

namespace StaticData
{
    /// <summary>
    /// 행동트리 에디터 저장 정보.
    /// BTData와 BTData를 상속받는 모든 클래스들의 정보를 담는다.
    /// </summary>
    [MemoryPackable]
    public partial class SDBehaviourEditorData
    {
        public BehaviourTree.BTEditorDataType Type { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public int ParentID { get; set; }
        public int ID { get; set; }
        public int ChildID { get; set; }
        public BehaviourTree.Execute ExecuteType { get; set; }
        public List<int> ChildrenID { get; set; }
        public int TrueNodeID { get; set; }
        public int FalseNodeID { get; set; }
        public BehaviourTree.Conditional ConditionalFuncType { get; set; }
        public int RepeatCount { get; set; }

        public BTData CreateBTData()
        {
            switch (Type)
            {
                case BehaviourTree.BTEditorDataType.Root:
                    return new BTRootData(ID, ChildID);
                case BehaviourTree.BTEditorDataType.Execute:
                    return new BTExecuteData(ID, ParentID, ExecuteType);
                case BehaviourTree.BTEditorDataType.Selector:
                    return new BTSelectorData(ID, ParentID, ChildrenID);
                case BehaviourTree.BTEditorDataType.Sequence:
                    return new BTSequenceData(ID, ParentID, ChildrenID);
                case BehaviourTree.BTEditorDataType.If:
                    return new BTIfData(ID, ParentID, TrueNodeID, FalseNodeID, ConditionalFuncType);
                case BehaviourTree.BTEditorDataType.While:
                    return new BTWhileData(ID, ParentID, ChildID, RepeatCount);
                default:
                    Debug.LogError($"Type : {Type} ID : {ID}");
                    return null;
            }
        }
    }
}