using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;

namespace Battle
{
    public class BattleManager : MonoSingleton<BattleManager>
    {
        private TileModule tileModule;

        public TileModule Tile => tileModule;
        
        protected override void Init()
        {
            tileModule = new TileModule();
        }

        private void FixedUpdate()
        {
            tileModule.UpdatePathFind();
        }
    }
}