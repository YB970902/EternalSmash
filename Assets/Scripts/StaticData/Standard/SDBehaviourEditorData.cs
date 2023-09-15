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
    public partial class SDBehaviourEditorData : StaticDataBase
    {
        /// <summary> 행동트리 타입 </summary>
        public BehaviourTree.BTEditorDataType Type { get; set; }
        /// <summary> 에디터상의 노드 좌표 X </summary>
        public float X { get; set; }
        /// <summary> 에디터상의 노드 좌표 Y </summary>
        public float Y { get; set; }
        /// <summary> 부모 노드 아이디 </summary>
        public int ParentID { get; set; }
        /// <summary> 자신의 노드 아이디 </summary>
        public int NodeID { get; set; }
        /// <summary> 자식 노드의 아이디 </summary>
        public int ChildID { get; set; }
        /// <summary> ExecuteNode에서 호출할 함수의 이름 </summary>
        public BehaviourTree.Execute ExecuteType { get; set; }
        /// <summary> 자식들 노드 아이디 </summary>
        public List<int> ChildrenID { get; set; }
        /// <summary> IfNode에서 호출한 함수의 결과가 참일경우 호출할 노드의 아이디 </summary>
        public int TrueNodeID { get; set; }
        /// <summary> IfNode에서 호출한 함수의 결과가 거짓일경우 호출할 노드의 아이디 </summary>
        public int FalseNodeID { get; set; }
        /// <summary> IfNode에서 호출할 함수의 이름 </summary>
        public BehaviourTree.Conditional ConditionalFuncType { get; set; }
        /// <summary> WhileNode에서 반복할 횟수 </summary>
        public int RepeatCount { get; set; }

        public BTData CreateBTData()
        {
            switch (Type)
            {
                case BehaviourTree.BTEditorDataType.Root:
                    return new BTRootData(NodeID, ChildID);
                case BehaviourTree.BTEditorDataType.Execute:
                    return new BTExecuteData(NodeID, ParentID, ExecuteType);
                case BehaviourTree.BTEditorDataType.Selector:
                    return new BTSelectorData(NodeID, ParentID, ChildrenID);
                case BehaviourTree.BTEditorDataType.Sequence:
                    return new BTSequenceData(NodeID, ParentID, ChildrenID);
                case BehaviourTree.BTEditorDataType.If:
                    return new BTIfData(NodeID, ParentID, TrueNodeID, FalseNodeID, ConditionalFuncType);
                case BehaviourTree.BTEditorDataType.While:
                    return new BTWhileData(NodeID, ParentID, ChildID, RepeatCount);
                default:
                    Debug.LogError($"Type : {Type} ID : {NodeID}");
                    return null;
            }
        }
    }
}