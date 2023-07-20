using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Battle;
using FixMath.NET;
using UnityEngine;
using System.Linq.Expressions;

public interface IPathFinder
{
    /// <summary>
    /// 초기화
    /// </summary>
    public void Init();

    /// <summary>
    /// 장애물 설정
    /// </summary>
    public void SetObstacle(int _index, bool _isObstacle);

    /// <summary>
    /// 경로 탐색 성공 여부를 반환하고, 경로는 매개변수를 통해 반환한다.
    /// </summary>
    public bool FindPath(List<int> _path, int _startIndex, int _destIndex);

    /// <summary>
    /// 목표 인덱스의 주변에서 시작 인덱스와 가장 가까운 열린 노드를 반환한다.
    /// </summary>
    public int GetNearOpenNode(int _startIndex, int _targetIndex);

    /// <summary>
    /// 점거중인지 설정
    /// </summary>
    public void SetOccupied(int _index, bool _isOccupied);

    /// <summary>
    /// 점거중인지 여부
    /// </summary>
    public bool IsOccupied(int _index);
}

/// <summary>
/// 타일과 관련된 비즈니스 로직이 있는 클래스이다.
/// 인게임에만 쓰이는 요소이므로, BattleManager에 의존적이다.
/// 길찾기 탐색 요청 응답 기능과 인덱스를 타일 위치로 바꿔주는 등의 기능을 제공한다.
/// </summary>
public class TileModule
{
    /// <summary>
    /// 길찾기 요청에 필요한 값을 가지고 있는 오브젝트이다.
    /// </summary>
    private class PathFindRequest
    {
        public PathGuide Guide { get; private set; }
        public List<int> Path { get; private set; }
        public int StartIndex { get; private set; }
        public int DestIndex { get; private set; }
        public System.Action OnPathFindEnd { get; private set; }
        
        /// <summary>
        /// 값을 세팅한다.
        /// </summary>
        public void Set(PathGuide _guide, List<int> _path, int _startIndex, int _destIndex, System.Action _onPathFindEnd)
        {
            Guide = _guide;
            Path = _path;
            StartIndex = _startIndex;
            DestIndex = _destIndex;
            OnPathFindEnd = _onPathFindEnd;
        }

        public void UpdateDestIndex(int _destIndex)
        {
            DestIndex = _destIndex;
        }
    }
    
    public const int WidthCount = 100;
    public const int HeightCount = 100;
    public const int TotalCount = WidthCount * HeightCount;

    private static readonly FixVector2 TileSize = new FixVector2(1, 1);
    private static readonly FixVector2 HalfTileSize = new FixVector2(0.5f, 0.5f);

    /// <summary> 화면에 위치할 수 있는 캐릭터의 최대 수. </summary>
    private const int CharacterPoolCount = 100;

    /// <summary>
    /// PathFuidRequest가 들어있는 오브젝트 풀.
    /// </summary>
    private ObjectPool<PathFindRequest> pathFindRequestPool;

    /// <summary> 경로 검색을 요청한 PathGuide </summary>
    private LinkedList<PathFindRequest> pathFindRequests;
    
    /// <summary> 경로 탐색기 </summary>
    private IPathFinder pathFinder;

    public TileModule()
    {
        pathFindRequestPool = new ObjectPool<PathFindRequest>(CharacterPoolCount);

        pathFindRequests = new LinkedList<PathFindRequest>();
        
        pathFinder = new FinderJPS();
        pathFinder.Init();
    }

    public void UpdatePathFind()
    {
        if (pathFindRequests.Count == 0) return;
        
        var requestObj = pathFindRequests.First.Value;
        pathFindRequests.RemoveFirst();

        pathFinder.FindPath(requestObj.Path, requestObj.StartIndex, requestObj.DestIndex);
        requestObj.OnPathFindEnd();
    }

    /// <summary>
    /// 길찾기를 요청한다. pathGuide는 pool에 들어가며, 매 프레임마다 조금씩 길찾기를 수행한다. 
    /// </summary>
    /// <param name="_pathGuide">길찾기를 수행할 PathGuide</param>
    /// <param name="_startIndex">출발지 인덱스</param>
    /// <param name="_destIndex">목적지 인덱스</param>
    public void RequestPathFind(PathGuide _guide, List<int> _path, int _startIndex, int _destIndex, System.Action _onPathFindEnd)
    {
        var requestObj = pathFindRequestPool.Pull();
        requestObj.Set(_guide, _path, _startIndex, _destIndex, _onPathFindEnd);
        pathFindRequests.AddLast(requestObj);
    }

    /// <summary>
    /// 길찾기를 취소한다.
    /// </summary>
    public void CancelPathFind(PathGuide _guide)
    {
        var requestObj = pathFindRequests.First(_ => _.Guide == _guide);
        pathFindRequests.Remove(requestObj);
        pathFindRequestPool.Push(requestObj);
    }

    /// <summary>
    /// 목적지 타일의 인덱스를 갱신한다.
    /// </summary>
    public void UpdateDestIndex(PathGuide _guide, int _destIndex)
    {
        var requestObj = pathFindRequests.First(_ => _.Guide == _guide);
        requestObj.UpdateDestIndex(_destIndex);
    }
    
    /// <summary>
    /// 인덱스에 맞는 타일의 위치 반환.
    /// </summary>
    public FixVector2 GetTilePosition(int _index)
    {
        if (_index < 0 || _index >= TotalCount)
        {
            Debug.LogError("Out of range");
            return FixVector2.Zero;
        }
        
        (int x, int y) = IndexToPosition(_index);
        
        return new FixVector2(TileSize.x * (Fix64)x + HalfTileSize.x, TileSize.y * (Fix64)y + HalfTileSize.y);
    }

    private (int, int) IndexToPosition(int _index)
    {
        if (_index < 0 || _index >= TotalCount)
        {
            Debug.LogError("Out of range");
            return (0, 0);
        }

        return (_index % WidthCount, _index / WidthCount);
    }

    public int GetNearOpenNode(int _startIndex, int _destIndex)
    {
        return pathFinder.GetNearOpenNode(_startIndex, _destIndex);
    }
    
    public static (int, int) IndexToPos(int _index)
    {
        return (_index % WidthCount, _index / HeightCount);
    }
    
    public static int PosToIndex(int _x, int _y)
    {
        return _x + _y * WidthCount;
    }

    public void SetOccupied(int _index, bool _isOccupied)
    {
        pathFinder.SetOccupied(_index, _isOccupied);
    }
    
    public bool IsOccupied(int _index)
    {
        return pathFinder.IsOccupied(_index);
    }
    
    #region Functions for test
    #if UNITY_EDITOR
    
    public IPathFinder PathFinder => pathFinder;
    
    public List<int> FindPathImmediately(int _startIndex, int _destIndex)
    {
        List<int> path = new List<int>(TileModule.TotalCount);
        pathFinder.FindPath(path, _startIndex, _destIndex);
        return path;
    }
    
    #endif
    #endregion
}
