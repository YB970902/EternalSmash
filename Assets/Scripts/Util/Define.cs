using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Define
{
    public class Tile
    {
        public const int InvalidTileIndex = -1;
        
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

        public static class Conditional
        {
            public const string True = "True";
            public const string False = "False";
        }
    }
}
