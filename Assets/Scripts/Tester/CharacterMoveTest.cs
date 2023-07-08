using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Character;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterMoveTest : MonoBehaviour
{
    [SerializeField] private int obstacleCount = 0;
    [SerializeField] private int nextNodeIndex = 0;
    [SerializeField] private MovementController movement = null;
    [SerializeField] private PathGuide guide = null;

    private void Start()
    {
        var mgr = BattleManager.Instance;
        guide.Init();
        guide.Set(movement);
        guide.SetStartIndex(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            guide.SetStartIndex(nextNodeIndex);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            guide.SetTargetIndex(nextNodeIndex);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            var pathFinder = TileManager.Instance.PathFinder;
            for (int i = 0; i < TileManager.TotalCount; ++i)
            {
                pathFinder.SetObstacle(i, false);
            }

            for (int i = 0; i < obstacleCount; ++i)
            {
                pathFinder.SetObstacle(Random.Range(1, TileManager.TotalCount - 1), true);
            }
        }
    }
}
