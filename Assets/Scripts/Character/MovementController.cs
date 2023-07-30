using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixMath.NET;
using UnityEditor.U2D.Animation;
using UnityEngine.Events;
using Battle;
using UnityEngine.UIElements;

namespace Character
{
    /// <summary>
    /// 
    /// </summary>
    public class MovementController : MonoBehaviour
    {
        /// <summary> 타일을 하나 건너는데 드는 틱 </summary>
        [SerializeField] private int moveSpeed = 100;
        [SerializeField] private int boostSpeed = 10;

        private int moveSpeed;
        
        /// <summary> 현재 이동중인지 여부. </summary>
        public bool IsMove { get; private set; }
        /// <summary> 현재 빠르게 이동중인지 여부 </summary>
        private bool isBoost;
        
        /// <summary> 현재 위치 </summary>
        public FixVector2 position;
        /// <summary> 이동을 시작한 위치 </summary>
        private FixVector2 startPosition;
        /// <summary> 이동해야할 목표 위치 </summary>
        private FixVector2 targetPosition;

        /// <summary> 이동을 시작한 위치의 인덱스 </summary>
        private int startIndex;
        /// <summary> 이해야할 목표 위치의 인덱스 </summary>
        private int targetIndex;

        /// <summary> 지금까지 카운트된 틱 </summary>
        private int currentCountTick;
        /// <summary> 최대 카운트 틱 </summary>
        private int maxCountTick;

        public void Init()
        {
            position = FixVector2.Zero;
            startPosition = FixVector2.Zero;
            targetPosition = FixVector2.Zero;
            startIndex = Define.Tile.InvalidTileIndex;
            targetIndex = Define.Tile.InvalidTileIndex;
            curCountTick = 0;
            currentCountTick = 0;
            IsMove = false;
            isBoost = false;
        }
        
        /// <summary>
        /// 해당 인덱스로 위치를 이동시킨다.
        /// 캐릭터가 생성되고나서 1회만 호출된다.
        /// </summary>
        public void SetStartPosition(int _index)
        {
            SetPosition(BattleManager.Instance.Tile.GetTilePosition(_index));
            startPosition = position;
            targetPosition = position;
            
            startIndex = _index;
            SetOccupied(startIndex, true);
        }

        /// <summary>
        /// 해당 포지션으로 위치를 이동시킨다
        /// </summary>
        private void SetPosition(FixVector2 _pos)
        {
            position = _pos;
            transform.position = new Vector2((float)_pos.x, (float)_pos.y);
        }

        /// <summary>
        /// 이동을 시작한다. 다음으로 이동할 타일의 인덱스를 받는다. 
        /// </summary>
        public void MoveStart(int _index, bool _isBoost = false)
        {
            startPosition = position;
            targetPosition = BattleManager.Instance.Tile.GetTilePosition(_index);
            IsMove = true;
            isBoost = _isBoost;
            maxCountTick = isBoost ? boostSpeed : moveSpeed;
            
            targetIndex = _index;
            SetOccupied(targetIndex, true);
        }

        /// <summary>
        /// 이동하도록 업데이트하는 함수. MoveStart가 먼저 호출되어야 한다.
        /// 다음 타일로 이동에 성공하면 True를 반환한다.
        /// </summary>
        public bool Tick()
        {
            if (IsMove == false) return false;
            
            ++currentCountTick;
            Fix64 t = (Fix64)currentCountTick / (Fix64)maxCountTick;
            SetPosition(FixVector2.Lerp(startPosition, targetPosition, t));
            if (currentCountTick >= maxCountTick)
            {
                IsMove = false;
                currentCountTick = 0;
                SetPosition(targetPosition);

                SetOccupied(startIndex, false);
                startIndex = targetIndex;
                if (isBoost)
                {
                    
                    isBoost = false;
                }

                return true;
            }

            return false;
        }
        
        /// <summary>
        /// 해당 타일의 점유 설정을 한다.
        /// </summary>
        private void SetOccupied(int _index, bool _isOccupied)
        {
            if (_index == Define.Tile.InvalidTileIndex) return;

            BattleManager.Instance.Tile.SetOccupied(_index, _isOccupied);
        }
    }
}