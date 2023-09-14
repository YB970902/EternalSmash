using System.Collections;
using System.Collections.Generic;
using MemoryPack;
using UnityEngine;

namespace StaticData
{
    [MemoryPackable]
    public partial class SDBehaviourEditorDataList
    {
        public List<SDBehaviourEditorData> EditorDatas;
    }
}