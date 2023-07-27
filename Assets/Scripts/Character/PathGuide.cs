using System.Collections;
using System.Collections.Generic;
using Battle;
using Define;

namespace Character
{

    /// <summary>
    /// 목표물까지의 이동경로를 알고있고, 경로가 잘못되었거나 이동중에 장애물 혹은 점유된 타일이 있는 등
    /// 여러가지 인터럽트가 발생했을 시 새로운 경로를 알아서 찾아낸다.
    /// MovementController에 접근하여 값을 주입하거나 반환받을 수 있다. 
    /// </summary>
    public class PathGuide
    {
        /// <summary> 이동을 시작하는 타일의 인덱스 </summary>
        private int startIndex;

        /// <summary> 도착해야하는 타일의 인덱스 </summary>
        private int targetIndex;

        /// <summary> 이동해야 하는 경로 </summary>
        private List<int> path = new List<int>(TileModule.TotalCount);

        /// <summary> 현재 경로의 인덱스 </summary>
        private int curPathIndex;

        /// <summary> 이동할 오브젝트의 이동 관리자 </summary>
        private MovementController controller;

        /// <summary> 대기하고 있었던 횟수 </summary>
        private int currentWaitCount;

        /// <summary> 다음 경로에 다른 유닛이 있어서 대기하고 있는지 여부 </summary>
        private bool isWaitMove;

        /// <summary> 경로를 받기 위해 대기중인지 여부 </summary>
        public bool IsWaitPath { get; private set; }

        /// <summary> 경로가 비어있는지 여부 </summary>
        private bool IsPathEmpty => path.Count == 0;

        /// <summary>
        /// 이동 관리자를 할당
        /// </summary>
        public void Init(MovementController _controller, int _startIndex)
        {
            startIndex = Define.Tile.InvalidTileIndex;
            targetIndex = Define.Tile.InvalidTileIndex;
            path.Clear();
            curPathIndex = Define.Tile.InvalidTileIndex;

            currentWaitCount = 0;
            isWaitMove = false;
            IsWaitPath = false;

            controller = _controller;
            SetStartIndex(_startIndex);
        }

        /// <summary>
        /// 이동 관리자 해제
        /// </summary>
        public void Relase()
        {
            controller = null;
            path.Clear();

            // 경로를 받기 위해 대기중인 경우 길찾기를 취소한다.
            if (IsWaitPath)
            {
                BattleManager.Instance.Tile.CancelPathFind(this);
                IsWaitPath = false;
            }
        }

        public Define.BehaviourTree.BTState Tick()
        {
            // 아직 길찾기 중인경우 Running을 반환한다.
            if (IsWaitPath) return BehaviourTree.BTState.Running;

            // 경로가 없다면 Fail을 반환한다.
            if (IsPathEmpty) return BehaviourTree.BTState.Fail;

            if (isWaitMove) // 다음 경로를 다른 유닛이 점유한 경우
            {
                if (IsOccupied(GetPath())) // 여전히 점유중인 경우
                {
                    // WaitMoveCount만큼 대기하다가 계속 점유중이면 새로운 경로를 찾는다.
                    ++currentWaitCount;
                    if (currentWaitCount < Define.Tile.WaitMoveCount) return BehaviourTree.BTState.Running;
                    currentWaitCount = 0;
                    SetTargetIndex(targetIndex, GetPath());
                    return BehaviourTree.BTState.Running;
                }

                // 다시 이동을 시작한다.
                isWaitMove = false;
                currentWaitCount = 0;
                controller.MoveStart(GetPath());
            }

            bool result = controller.Tick();

            // 아직 이동중이라면 Running을 반환한다.
            if (result == false) return BehaviourTree.BTState.Running;

            // 이동 전 위치는 점유를 해제한다.
            startIndex = GetPath();
            ++curPathIndex;

            if (path.Count <= curPathIndex) return BehaviourTree.BTState.Fail; // 최종 목적지에 도착하면 Fail을 반환한다.

            if (IsOccupied(GetPath())) // 다음 노드가 점유하고 있을경우
            {
                // 다음 노드의 점유가 풀릴때까지 대기한다.
                isWaitMove = true;
                currentWaitCount = 0;
                return BehaviourTree.BTState.Success;
            }

            controller.MoveStart(GetPath());

            return BehaviourTree.BTState.Success;
        }

        /// <summary>
        /// 시작 위치 설정
        /// 관리중인 오브젝트도 해당 위치로 이동시킨다.
        /// </summary>
        private void SetStartIndex(int _index)
        {
            startIndex = _index;
            controller.SetStartPosition(_index);
        }

        /// <summary>
        /// 이동할 목표 위치를 설정한다.
        /// </summary>
        public void SetTargetIndex(int _index, int _tempObstacleIndex = Tile.InvalidTileIndex)
        {
            targetIndex = BattleManager.Instance.Tile.GetNearOpenNode(startIndex, _index);

            if (IsWaitPath)
            {
                // 이미 경로를 받기위해 대기중이라면 목적지를 갱신한다. 
                BattleManager.Instance.Tile.UpdateDestIndex(this, targetIndex, _tempObstacleIndex);
                return;
            }

            // 길찾기 요청을 보낸다.
            BattleManager.Instance.Tile.RequestPathFind(this, path, startIndex, targetIndex, OnFindPath, _tempObstacleIndex);

            // 경로를 기다린다.
            IsWaitPath = true;
        }

        /// <summary>
        /// 길찾기를 마친 경우 호출.
        /// </summary>
        private void OnFindPath()
        {
            // 더이상 경로를 기다리지 않는다.
            IsWaitPath = false;
            isWaitMove = false;

            // 경로가 없을경우 반환한다.
            // TODO : 반환보다는 일정시간 대기후에 다시 길찾기를 시도하는게 좋아보인다.
            if (path.Count == 0) return;

            curPathIndex = 0;
            if (IsPathEmpty == false)
            {
                if (IsOccupied(GetPath()))
                {
                    isWaitMove = true;
                }
                else
                {
                    controller.MoveStart(GetPath());
                }
            }
        }

        private int GetPath()
        {
            if (path.Count <= curPathIndex) return Define.Tile.InvalidTileIndex;
            return path[curPathIndex];
        }

        private bool IsOccupied(int _index)
        {
            if (_index == Define.Tile.InvalidTileIndex) return true;

            return BattleManager.Instance.Tile.IsOccupied(_index);
        }
    }
}