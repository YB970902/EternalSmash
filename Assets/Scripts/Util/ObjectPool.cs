using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : class, new()
{
    private T[] pool;
    private int count;
    private readonly int maxCount;

    /// <summary> 현재 보유하고 있는 오브젝트 수 </summary>
    public int Count => count;

    public ObjectPool(int _count)
    {
        count = _count;
        maxCount = _count;
        pool = new T[_count];
        
        for (int i = 0; i < _count; ++i)
        {
            pool[i] = new T();
        }
    }

    /// <summary>
    /// 오브젝트를 반환한다.
    /// </summary>
    public T Pull()
    {
        if (count <= 0)
        {
            Debug.LogError($"ObjectPool<{nameof(T)}> is Empty. MaxCount : {maxCount}");
            return null;
        }
        return pool[--count];
    }

    /// <summary>
    /// 오브젝트를 집어넣는다.
    /// </summary>
    public void Push(T _data)
    {
        pool[count++] = _data;
    }
}
