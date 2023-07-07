using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PathFindTest : MonoBehaviour
{
    [SerializeField] TileTest prefabTile = null;

    [SerializeField] int obstacleCount = 0;

    [SerializeField] private List<int> listObstacle = null;

    [SerializeField] private int testCount = 10;

    private List<TileTest> tileTestList = new List<TileTest>(TileManager.WidthCount * TileManager.HeightCount);
    
    void Start()
    {
        for(int y = 0; y < TileManager.HeightCount; ++y)
        {
            for(int x = 0; x < TileManager.WidthCount; ++x)
            {
                var tile = Instantiate(prefabTile);
                tile.SetColor(TileTest.TileTestColorTag.Default);
                tile.SetTilePosition(x + y * TileManager.WidthCount);
                tileTestList.Add(tile);
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            PathFindingTest();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PathFindTimeTest();
        }
    }

    private void PathFindingTest()
    {
        int startIndex = 0;
        int destIndex = TileManager.WidthCount * TileManager.HeightCount - 1;

        for (int y = 0; y < TileManager.HeightCount; ++y)
        {
            for (int x = 0; x < TileManager.WidthCount; ++x)
            {
                var index = x + y * TileManager.WidthCount;

                TileManager.Instance.PathFinder.SetObstacle(index, false);
                tileTestList[index].SetColor(TileTest.TileTestColorTag.Default);
            }
        }

        if (listObstacle == null || listObstacle.Count == 0)
        {
            for (int i = 0; i < obstacleCount; ++i)
            {
                int randomIndex = Random.Range(0, TileManager.WidthCount * TileManager.HeightCount);

                if (randomIndex == startIndex || randomIndex == destIndex) continue;

                TileManager.Instance.PathFinder.SetObstacle(randomIndex, true);
                tileTestList[randomIndex].SetColor(TileTest.TileTestColorTag.Obstacle);
            }
        }
        else
        {
            for (int i = 0; i < listObstacle.Count; ++i)
            {
                TileManager.Instance.PathFinder.SetObstacle(listObstacle[i], true);
                tileTestList[listObstacle[i]].SetColor(TileTest.TileTestColorTag.Obstacle);
            }
        }


        var path = TileManager.Instance.PathFinder.FindPath(startIndex, destIndex);

        path?.ForEach(index => tileTestList[index].SetColor(TileTest.TileTestColorTag.Path));
        tileTestList[startIndex].SetColor(TileTest.TileTestColorTag.Start);
        tileTestList[destIndex].SetColor(TileTest.TileTestColorTag.Dest);
    }

    private void PathFindTimeTest()
    {
        int startIndex = 0;
        int destIndex = TileManager.WidthCount * TileManager.HeightCount - 1;

        for (int y = 0; y < TileManager.HeightCount; ++y)
        {
            for (int x = 0; x < TileManager.WidthCount; ++x)
            {
                var index = x + y * TileManager.WidthCount;

                TileManager.Instance.PathFinder.SetObstacle(index, false);
                tileTestList[index].SetColor(TileTest.TileTestColorTag.Default);
            }
        }

        var stopWatch = new Stopwatch();
        int curCount = 0;
        long totalTime = 0;
        while (curCount < testCount)
        {
            for (int i = 0; i < TileManager.TotalCount; ++i)
            {
                TileManager.Instance.PathFinder.SetObstacle(i, false);
            }
            
            for (int i = 0; i < obstacleCount; ++i)
            {
                int randomIndex = Random.Range(0, TileManager.WidthCount * TileManager.HeightCount);

                if (randomIndex == startIndex || randomIndex == destIndex) continue;

                TileManager.Instance.PathFinder.SetObstacle(randomIndex, true);
            }
            
            stopWatch.Start();
            
            var path = TileManager.Instance.PathFinder.FindPath(startIndex, destIndex);
            
            stopWatch.Stop();

            if (path == null) continue;

            ++curCount;
            totalTime += stopWatch.ElapsedMilliseconds;
        }
        
        Debug.Log($"평균 시간 : {totalTime / testCount}ms");
    }
}
