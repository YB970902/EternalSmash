using System.Collections;
using System.Collections.Generic;
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
    public static int WidthCount = 100;
    public static int HeightCount = 100;

    public static int TotalCount = WidthCount * HeightCount;

    private IPathFinder pathFinder;

    public IPathFinder PathFinder => pathFinder;

    public override void Init()
    {
        pathFinder = new FinderJPS();
        pathFinder.Init();
    }
}
