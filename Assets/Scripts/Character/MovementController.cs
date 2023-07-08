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
    /// 이동을 관리하는 스크립트. TileManager에 의존적이다.
    /// 외부에서 index를 입력받으면 TileManager를 통해 이동할 경로를 알아낸다.
    /// 직접 이동할 곳을 찾거나 하진 않고 외부에서 입력받도록 설계되었다.
    /// </summary>
    public class MovementController : MonoBehaviour, IMovable
    {
        /// <summary> 타일을 하나 건너는데 드는 틱 </summary>
        [SerializeField] private int moveSpeed = 100;
        
        /// <summary> 현재 이동중인지 여부. </summary>
        public bool IsMove { get; private set; }

        /// <summary> 현재 위치 </summary>
        public FixVector2 position;
        /// <summary> 이동을 시작한 위치 </summary>
        private FixVector2 startPosition;
        /// <summary> 이동해야할 목표 위치 </summary>
        private FixVector2 targetPosition;

        /// <summary> 지금까지 카운트된 틱 </summary>
        private int curCountTick;

        /// <summary> 목표에 도착했을 경우 호출되는 이벤트 </summary>
        public UnityEvent onArrive { get; private set; }

        public void Init()
        {
            position = FixVector2.Zero;
            startPosition = FixVector2.Zero;
            targetPosition = FixVector2.Zero;
            curCountTick = 0;
            IsMove = false;
            onArrive = new UnityEvent();
        }

        /// <summary>
        /// 위치를 이동시킨다
        /// </summary>
        public void SetPosition(FixVector2 pos)
        {
            position = pos;
            transform.position = new Vector2((float)pos.x, (float)pos.y);
        }

        /// <summary>
        /// 다음으로 이동해야 할 타일을 지정한다.
        /// </summary>
        public void SetNextTile(int _index)
        {
            startPosition = position;
            targetPosition = TileManager.Instance.GetTilePosition(_index);
            IsMove = true;
        }

        /// <summary>
        /// 위치를 이동시킨다.
        /// </summary>
        public void SetPosition(int _index)
        {
            SetPosition(TileManager.Instance.GetTilePosition(_index));
            startPosition = position;
            targetPosition = position;
        }

        /// <summary>
        /// 목적지에 도착했을때 호출
        /// </summary>
        private void OnArriveToTargetPosition()
        {
            IsMove = false;
            SetPosition(targetPosition);
            onArrive.Invoke();
        }

        public void Tick()
        {
            if (IsMove == false) return;
            
            ++curCountTick;
            Fix64 t = (Fix64)curCountTick / (Fix64)moveSpeed;
            SetPosition(FixVector2.Lerp(startPosition, targetPosition, t));
            if (curCountTick >= moveSpeed)
            {
                IsMove = false;
                curCountTick = 0;
                OnArriveToTargetPosition();
            }
        }
    }
}