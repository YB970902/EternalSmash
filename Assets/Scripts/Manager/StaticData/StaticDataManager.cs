using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MemoryPack;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEditor;
using UnityEngine;

namespace StaticData
{
    /// <summary>
    /// 고정 데이터 관리자.
    /// 인스턴스를 byte 파일로 만들어 저장하고, 저장된 byte 파일을 인스턴스로 만드는 기능을 제공한다.
    /// </summary>
    public static class StaticDataManager
    {
        /// <summary>
        /// 데이터를 직렬화한 뒤에 데이터를 저장한다.
        /// </summary>
        public static void Save<T>(List<T> _data, string _name = null) where T: StaticDataBase
        {
            var type = typeof(T);
            var path = GetPath(type, _name);

            CheckFolderExistAndCreate(path, type, _name);

            var collection = new StaticDataCollection<T> { DataList = _data };

            using var fileStream = File.Open(path, FileMode.Create, FileAccess.Write);
            
            var bytes = MemoryPackSerializer.Serialize(collection);
            fileStream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// 파일을 로드하고 데이터를 반환한다.
        /// 경로가 없으면 null을 반환하며, 데이터가 없으면 익셉션을 발생시킨다.
        /// </summary>
        public static StaticDataCollection<T> Load<T>(string _name = null) where T: StaticDataBase
        {
            var type = typeof(T);
            var path = GetPath(type, _name);

            if (IsFolderExist(path, type, _name) == false) return null;

            var assetPath = path.Substring(path.LastIndexOf("Asset", StringComparison.Ordinal));
            var textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath);
            
            StaticDataCollection<T> result = null;
            
            try
            {
                MemoryPackSerializer.Deserialize(textAsset.bytes, ref result);
            }
            catch(Exception e)
            {
                Debug.LogError($"StaticData Load Error! [{e.Message}] - Path : {assetPath}");
                throw;
            }

            return result;
        }

        /// <summary>
        /// 드라이브에서부터의 경로를 반환한다.
        /// </summary>
        /// <param name="_type"> 데이터의 타입 </param>
        /// <param name="_name"> 데이터의 이름 </param>
        /// <returns> 드라이브에서부터의 경로 </returns>
        private static string GetPath(Type _type, string _name)
        {
            var sb = new StringBuilder();
            sb.Append(Define.Path.StaticDataFolder);
            sb.Append("/");
            sb.Append(_type.Name);
            sb.Append("/");
            sb.Append(_name ?? _type.Name);
            sb.Append(".bytes");
            return sb.ToString();
        }

        /// <summary>
        /// 폴더가 있는지 여부
        /// </summary>
        /// <param name="_fullPath"> 드라이브에서부터 시작하는 경로 </param>
        /// <param name="_type"> 데이터의 타입 </param>
        /// <param name="_name"> 데이터의 이름 </param>
        /// <returns> 폴더의 존재 여부 </returns>
        private static bool IsFolderExist(string _fullPath, Type _type, string _name)
        {
            var assetPath = _fullPath.Substring(_fullPath.LastIndexOf("Assets", StringComparison.Ordinal));
            var folderPath = assetPath.Substring(0, assetPath.LastIndexOf(_name ?? _type.Name, StringComparison.Ordinal));
            
            return Directory.Exists(folderPath);
        }

        /// <summary>
        /// 폴더가 있는지 체크하고 없으면 폴더를 생성한다.
        /// </summary>
        /// <param name="_fullPath"> 드라이브에서부터 시작하는 경로 </param>
        /// <param name="_type"> 데이터의 타입 </param>
        /// <param name="_name"> 데이터의 이름 </param>
        private static void CheckFolderExistAndCreate(string _fullPath, Type _type, string _name)
        {
            var assetPath = _fullPath.Substring(_fullPath.LastIndexOf("Assets", StringComparison.Ordinal));
            var folderPath = assetPath.Substring(0, assetPath.LastIndexOf(_name ?? _type.Name, StringComparison.Ordinal));

            // 폴더가 없으면 만든다.
            if (IsFolderExist(_fullPath, _type, _name) == false)
            {
                Directory.CreateDirectory(folderPath);
            }
        }
    }
}