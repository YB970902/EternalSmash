using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Define
{
    public static class Path
    {
        public static readonly string AssetFolder = Application.dataPath;
        public static readonly string StaticDataFolder = $"{AssetFolder}/StaticData";
    }
    
    public static class Tile
    {
        public const int InvalidTileIndex = -1;

        /// <summary> 다음 노드의 점유가 풀릴때까지 대기하는 수 </summary>
        public const int WaitMoveCount = 10;
        
        public enum Direct
        {
            Start = -1,
            Up,
            Down,
            Left,
            Right,
            End,
        }

        public enum DiagonalDirect
        {
            Start = -1,
            LeftUp,
            RightUp,
            LeftDown,
            RightDown,
            End,
        }
    }

    public static class BehaviourTree
    {
        /// <summary> 유효하지 않은 아이디 </summary>
        public const int InvalidID = 0;

        /// <summary> 유효한 아이디의 시작 값 </summary>
        public const int ValidStartID = 1;
        
        public enum BTState
        {
            Success,
            Fail,
            Running,
        }

        [Flags]
        public enum BTDebuggerFlag
        {
            None = 0,
            CheckSuccess = 1 << 0,
            CheckFail = 1 << 1,
            CheckRunning = 1 << 2,
        }

        public enum BTNodeType
        {
            Root,
            Control,
            Execute,
        }

        /// <summary>
        /// 행동트리 에디터 데이터의 종류
        /// </summary>
        public enum BTEditorDataType
        {
            Root,
            Execute,
            Selector,
            Sequence,
            If,
            While,
        }

        public enum BTControlNodeType
        {
            None,
            Sequence,
            Selector,
            If,
            While,
        }

        public enum Conditional
        {
            None,
            True,
            False,
            HasTarget,
            IsArrived,
            End,
        }

        public enum Execute
        {
            None,
            FollowTarget,
            MoveToTarget,
            TwoTickSuccess,
            TwoTickFail,
            FindPathRandomTarget,
            SetRandomTargetIndex,
            Move,
            Idle,
            End,
        }
    }
}
