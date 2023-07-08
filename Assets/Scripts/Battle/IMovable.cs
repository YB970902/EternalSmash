using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    /// <summary>
    /// 이동할 수 있는 오브젝트가 구현해야 하는 인터페이스
    /// BattleManager가 이동, 공격, 사망 등의 처리를 일괄적으로 하기 위해서 존재한다.
    /// </summary>
    public interface IMovable
    {
        /// <summary>
        /// 이동이 업데이트 되는 타이밍에 호출된다.
        /// </summary>
        public void Tick();
    }
}
