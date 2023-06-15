using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var path = TileManager.Instance.FindPath(0, 5);

        Debug.Log($"PathCount : {path.Count}");
        path.ForEach(_ => Debug.Log(_));
    }
}
