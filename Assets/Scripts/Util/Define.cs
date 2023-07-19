using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Define
{
    public class Tile
    {
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


        public static class Conditional
        {
            public const string True = "True";
            public const string False = "False";
        }
    }
}
