using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;

/// <summary>
/// MovementController에 경로를 주입하는 가이드
/// 시작위치와 도착위치를 알고 있고 다음으로 이동할 경로를 지속적으로 주입한다.
/// 다음경로가 막혀있을때나 특정 주기가 되었을 때 새 경로를 탐색한다.
/// TileManager를 통해 관리된다.
/// </summary>
public class PathGuide : MonoBehaviour
{
    /// <summary> 이동을 시작하는 타일의 인덱스 </summary>
    private int startIndex;
    /// <summary> 도착해야하는 타일의 인덱스 </summary>
    private int targetIndex;

    /// <summary> 이동해야 하는 경로 </summary>
    private List<int> path = new List<int>(TileManager.TotalCount);
    /// <summary>
    /// 현재 경로의 인덱스.
    /// 경로의 앞부분을 자꾸 지우면 연산이 크기 때문에 인덱스를 사용한다.
    /// </summary>
    private int curPathIndex;

    /// <summary> 이동할 오브젝트의 이동 관리자 </summary>
    private MovementController controller;

    /// <summary>
    /// 경로를 받기 위해 대기중인지 여부
    /// </summary>
    private bool isWaitPath;
    
    public bool IsEnable { get; private set; }
    
    /// <summary>
    /// 초기화
    /// </summary>
    private void Init()
    {
        startIndex = 0;
        targetIndex = 0;
        path.Clear();
        curPathIndex = 0;
        
        isWaitPath = false;
        IsEnable = true;
        
        if (controller != null)
        {
            controller.onArrive.RemoveListener(OnArriveTarget);
            controller = null;
        }
    }

    /// <summary>
    /// 이동 관리자를 할당
    /// </summary>
    public void Set(MovementController _controller)
    {
        Init();
        controller = _controller;
        _controller.onArrive.AddListener(OnArriveTarget);
    }

    /// <summary>
    /// 이동 관리자 해제
    /// </summary>
    public void Relase()
    {
        controller.onArrive.RemoveListener(OnArriveTarget);
        controller = null;
        path.Clear();
        IsEnable = false;
        
        // 경로를 받기 위해 대기중인 경우 길찾기를 취소한다.
        if (isWaitPath)
        {
            TileManager.Instance.CancelPathFind(this);
            isWaitPath = false;
        }
    }

    /// <summary>
    /// 시작 위치 설정
    /// 관리중인 오브젝트도 해당 위치로 이동시킨다.
    /// </summary>
    public void SetStartIndex(int _index)
    {
        startIndex = _index;
        controller.SetPosition(_index);
    }

    /// <summary>
    /// 이동할 목표 위치를 설정한다.
    /// </summary>
    public void SetTargetIndex(int _index)
    {
        targetIndex = _index;

        if (isWaitPath)
        {
            TileManager.Instance.CancelPathFind(this);
        }

        // 길찾기 요청을 보낸다.
        TileManager.Instance.RequestPathFind(this, path, startIndex, targetIndex, OnFindPath);
        
        // 경로를 기다린다.
        isWaitPath = true;
    }

    /// <summary>
    /// 다음 타일에 도착했을 때
    /// </summary>
    private void OnArriveTarget()
    {
        startIndex = path[curPathIndex];
        ++curPathIndex;
        // 최종 목적지에 도착
        if (path.Count <= curPathIndex) return;
        
        controller.SetNextTile(path[curPathIndex]);
    }

    /// <summary>
    /// 길찾기를 마친 경우 호출.
    /// </summary>
    private void OnFindPath()
    {
        // 더이상 경로를 기다리지 않는다.
        isWaitPath = false;
        
        // 경로가 없을경우 반환한다.
        // TODO : 반환보다는 일정시간 대기후에 다시 길찾기를 시도하는게 좋아보인다.
        if (path.Count == 0) return;

        curPathIndex = 0;
        controller.SetNextTile(path[curPathIndex]);
    }
}
