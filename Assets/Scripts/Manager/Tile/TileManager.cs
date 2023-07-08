using System.Collections;
using System.Collections.Generic;
using FixMath.NET;
using UnityEngine;

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
    /// 경로 반환
    /// </summary>
    public List<int> FindPath(int _startIndex, int _destIndex);
}

public class TileManager : MonoSingleton<TileManager>
{
    public const int WidthCount = 100;
    public const int HeightCount = 100;
    public const int TotalCount = WidthCount * HeightCount;

    private static readonly FixVector2 TileSize = new FixVector2(1, 1);
    private static readonly FixVector2 HalfTileSize = new FixVector2(0.5f, 0.5f);
    
    private IPathFinder pathFinder;

    public IPathFinder PathFinder => pathFinder;

    public override void Init()
    {
        pathFinder = new FinderAstar();
        pathFinder.Init();
    }

    /// <summary>
    /// 타일의 위치 반환.
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
}
