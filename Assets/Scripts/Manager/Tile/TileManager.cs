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
    public void SetObstacle(int index, bool isObstacle);

    /// <summary>
    /// 경로 반환
    /// </summary>
    public List<int> FindPath(int startIndex, int destIndex);
}

public class TileManager : MonoSingleton<TileManager>
{
    public static int WidthCount = 10;
    public static int HeightCount = 10;

    private IPathFinder pathFinder;

    public IPathFinder PathFinder => pathFinder;

    public override void Init()
    {
        pathFinder = new FinderAstar();
        pathFinder.Init();
    }
}
