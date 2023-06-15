using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPathFinder
{
    public void Init();
    public List<int> FindPath(int startIndex, int destIndex);
}

public class TileManager : MonoSingleton<TileManager>
{
    public static int TileXCount = 10;
    public static int TileYCount = 10;

    private IPathFinder pathFinder;

    public override void Init()
    {
        pathFinder = new FinderAstar();
        pathFinder.Init();
    }

    public List<int> FindPath(int startIndex, int destIndex)
    {
        return pathFinder.FindPath(startIndex, destIndex);
    }
}
