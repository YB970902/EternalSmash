using System.Collections.Generic;
using MemoryPack;
using UnityEngine;

namespace StaticData
{
    [MemoryPackable]
    public partial class StaticDataCollection<T> where T : StaticDataBase
    {
        public List<T> DataList { get; set; }

        private Dictionary<string, T> dataDictionary;

        [MemoryPackIgnore]
        public T this[string _nameID] => !string.IsNullOrEmpty(_nameID) && dataDictionary.ContainsKey(_nameID) ? dataDictionary[_nameID] : null;
    }
}