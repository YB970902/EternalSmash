using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Define
{
    public class Tile
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

    public class BehaviourTree
    {
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

        public enum BTControlNodeType
        {
            None,
            Sequence,
            Selector,
            If,
            While,
        }

        public enum BTConditional
        {
            None,
            True,
            False,
            End,
        }

        public enum BTExecute
        {
            None,
            End,
        }
    }
}
