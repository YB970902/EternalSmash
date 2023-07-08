using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Character;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterMoveTest : MonoBehaviour
{
    [SerializeField] private int nextNodeIndex = 0;
    [SerializeField] private MovementController movement;

    private void Start()
    {
        var mgr = BattleManager.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            movement.SetPosition(nextNodeIndex);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            movement.SetNextTile(nextNodeIndex);
        }
    }
}
