using System.Collections;
using System.Collections.Generic;
using Battle;
using Character;
using Define;
using UnityEngine;

/// <summary>
/// MovementController에 경로를 주입하는 가이드
/// 시작위치와 도착위치를 알고 있고 다음으로 이동할 경로를 지속적으로 주입한다.
/// 다음경로가 막혀있을때나 특정 주기가 되었을 때 새 경로를 탐색한다.
/// TileManager를 통해 관리된다.
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

    /// <summary> 경로를 받기 위해 대기중인지 여부 </summary>
    public bool IsWaitPath { get; private set; }

    /// <summary> 이동할 준비가 되었는지 여부. </summary>
    public bool IsReadyToMove { get; private set; }
    
    /// <summary> 이동중인지 여부 </summary>
    public bool IsMove => controller.IsMove;

    /// <summary>
    /// 초기화
    /// </summary>
    private void Init()
    {
        startIndex = Define.Tile.InvalidTileIndex;
        targetIndex = Define.Tile.InvalidTileIndex;
        path.Clear();
        curPathIndex = Define.Tile.InvalidTileIndex;
        
        IsWaitPath = false;
        IsReadyToMove = false;
    }

    /// <summary>
    /// 이동 관리자를 할당
    /// </summary>
    public void Set(MovementController _controller, int _startIndex)
    {
        Init();
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

        bool result = controller.Tick();

        // 아직 이동중이라면 Running을 반환한다.
        if (result == false) return BehaviourTree.BTState.Running;

        // 이동을 마친경우, 시작 인덱스를 수정한다.
        startIndex = GetPath();
        ++curPathIndex;
        
        // 최종 목적지에 도착
        if (path.Count <= curPathIndex)
        {
            // 최종 목적지에 도착하면 Fail을 반환한다.
            return BehaviourTree.BTState.Fail;
        }

        // 다음 경로 설정
        controller.SetNextTile(GetPath());

        return BehaviourTree.BTState.Success;
    }

    /// <summary>
    /// 시작 위치 설정
    /// 관리중인 오브젝트도 해당 위치로 이동시킨다.
    /// </summary>
    private void SetStartIndex(int _index)
    {
        if (Define.Tile.InvalidTileIndex != startIndex)
        {
            // 이미 어딘가에 위치해 있는 상황에서 순간이동이라면, 점거를 해제해야 한다.
            SetOccupied(startIndex, false);
            SetOccupied(GetPath(), false);
        }
        startIndex = _index;
        controller.SetPosition(_index);
        SetOccupied(startIndex, true);
    }

    /// <summary>
    /// 이동할 목표 위치를 설정한다.
    /// </summary>
    public void SetTargetIndex(int _index)
    {
        targetIndex = BattleManager.Instance.Tile.GetNearOpenNode(startIndex, _index);

        if (IsWaitPath)
        {
            // 이미 경로를 받기위해 대기중이라면 목적지를 갱신한다. 
            BattleManager.Instance.Tile.UpdateDestIndex(this, targetIndex);
            return;
        }

        // 길찾기 요청을 보낸다.
        BattleManager.Instance.Tile.RequestPathFind(this, path, startIndex, targetIndex, OnFindPath);
        
        // 경로를 기다린다.
        IsWaitPath = true;
        IsReadyToMove = false;
    }

    /// <summary>
    /// 길찾기를 마친 경우 호출.
    /// </summary>
    private void OnFindPath()
    {
        // 더이상 경로를 기다리지 않는다.
        IsWaitPath = false;
        // 이동할 준비가 되었다.
        IsReadyToMove = true;
        
        // 경로가 없을경우 반환한다.
        // TODO : 반환보다는 일정시간 대기후에 다시 길찾기를 시도하는게 좋아보인다.
        if (path.Count == 0) return;
        IsReadyToMove = true;
    }

    /// <summary>
    /// 이동을 시작하기위해 세팅한다.
    /// </summary>
    public void SetMoveStart()
    {
        curPathIndex = 0;
        startIndex = GetPath();
        controller.SetNextTile(path[curPathIndex]);
        IsReadyToMove = false;
    }

    private int GetPath()
    {
        if (path.Count <= curPathIndex) return Define.Tile.InvalidTileIndex;
        return path[curPathIndex];
    }

    private void SetOccupied(int _index, bool _isOccupied)
    {
        if (_index == Define.Tile.InvalidTileIndex) return;
        
        BattleManager.Instance.Tile.SetOccupied(_index, _isOccupied);
    }
    
    private bool IsOccupied(int _index)
    {
        if (_index == Define.Tile.InvalidTileIndex) return true;
        
        return BattleManager.Instance.Tile.IsOccupied(_index);
    }
}
