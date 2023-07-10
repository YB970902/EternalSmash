using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;

namespace Battle
{
    public class BattleManager : MonoSingleton<BattleManager>
    {
        [SerializeField] private List<MovementController> movementControllers;
        public override void Init()
        {
            for (int i = 0, count = movementControllers.Count; i < count; ++i)
            {
                movementControllers[i].Init();
            }
        }

        private void FixedUpdate()
        {
            TileManager.Instance.UpdatePathFind();

            UpdateMovementObject();
        }

        private void UpdateMovementObject()
        {
            for (int i = 0, count = movementControllers.Count; i < count; ++i)
            {
                movementControllers[i].Tick();
            }
        }
    }
}