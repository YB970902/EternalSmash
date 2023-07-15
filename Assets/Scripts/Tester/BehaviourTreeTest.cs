using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeTest : MonoBehaviour
{
    void Start()
    {
        var builder = new BTBuilder();
        builder.AddRootNode(BTRootData.Create(builder, 1, 2)).Build();
    }
}
