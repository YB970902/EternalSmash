using System.Collections;
using System.Collections.Generic;
using MemoryPack;
using UnityEngine;

namespace StaticData
{
    [MemoryPackable]
    public partial class StaticDataBase
    {
        /// <summary> 데이터의 고유 아이디 </summary>
        public int ID { get; set; }

        /// <summary> 데이터의 문자열 아이디 </summary>
        public string NameID { get; set; }
    }
}