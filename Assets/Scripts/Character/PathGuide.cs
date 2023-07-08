using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;

/// <summary>
/// MovementController에 경로를 주입하는 가이드
/// 시작위치와 도착위치를 알고 있고 다음으로 이동할 경로를 지속적으로 주입한다.
/// 다음경로가 막혀있을때나 특정 주기가 되었을 때 새 경로를 탐색한다.
/// </summary>
public class PathGuide : MonoBehaviour
{
    /// <summary> 이동을 시작하는 타일의 인덱스 </summary>
    private int startIndex;
    /// <summary> 도착해야하는 타일의 인덱스 </summary>
    private int targetIndex;

    /// <summary> 이동해야 하는 경로 </summary>
    private List<int> path;
    /// <summary>
    /// 현재 경로의 인덱스.
    /// 경로의 앞부분을 자꾸 지우면 연산이 크기 때문에 인덱스를 사용한다.
    /// </summary>
    private int curPathIndex;

    /// <summary> 이동할 오브젝트의 이동 관리자 </summary>
    private MovementController controller;

    public void Init()
    {
        startIndex = 0;
        targetIndex = 0;
        if (controller != null)
        {
            controller.onArrive.RemoveListener(OnArriveTarget);
        }
        controller = null;
        path = null;
        curPathIndex = 0;
    }

    public void Set(MovementController _controller)
    {
        controller = _controller;
        _controller.onArrive.AddListener(OnArriveTarget);
    }

    public void Relase()
    {
        controller.onArrive.RemoveListener(OnArriveTarget);
        controller = null;
        path = null;
    }

    public void SetStartIndex(int _index)
    {
        startIndex = _index;
        controller.SetPosition(_index);
    }

    public void SetTargetIndex(int _index)
    {
        targetIndex = _index;
        /*
         * TODO : 목적지를 주입받았다고 해서 계속 경로를 재탐색한다면 이슈가 생길 수 있다. 일단 목적지를 받아두고 어느정도 주기가 지나거나 특정 조건을 만족했을때 재탐색해야 한다.
         * 지금은 당장 재탐색을 한다. 추후에 재탐색할 조건과 주기를 선택하자.
         */

        var newPath = TileManager.Instance.PathFinder.FindPath(startIndex, targetIndex);
        
        if (newPath == null) return;

        path = newPath;

        curPathIndex = 0;
        controller.SetNextTile(path[curPathIndex]);
    }

    /// <summary>
    /// 다음 타일에 도착했을 때
    /// </summary>
    private void OnArriveTarget()
    {
        ++curPathIndex;
        // 최종 목적지에 도착
        if (path.Count <= curPathIndex) return;
        
        controller.SetNextTile(path[curPathIndex]);
    }
}
