using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Battle;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PathFindTest : MonoBehaviour
{
    [SerializeField] TileTest prefabTile = null;
    [SerializeField] CharacterTest prefabCharacter = null;

    [SerializeField] int obstacleCount = 0;

    [SerializeField] private List<int> listObstacle = null;

    [SerializeField] private int testCount = 10;

    private List<TileTest> tileTestList = new List<TileTest>(TileModule.WidthCount * TileModule.HeightCount);
    
    void Start()
    {
        Random.InitState(100);
        
        for(int y = 0; y < TileModule.HeightCount; ++y)
        {
            for(int x = 0; x < TileModule.WidthCount; ++x)
            {
                var tile = Instantiate(prefabTile);
                tile.SetColor(TileTest.TileTestColorTag.Default);
                tile.SetTilePosition(x + y * TileModule.WidthCount);
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

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SpawnCharacter();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CreateRandomObstacle();
        }
    }

    private void CreateRandomObstacle(List<int> _ignoreIndexList = null)
    {
        for (int i = 0; i < obstacleCount; ++i)
        {
            int randomIndex = Random.Range(0, TileModule.WidthCount * TileModule.HeightCount);

            if (_ignoreIndexList?.Contains(randomIndex) ?? false) continue;

            BattleManager.Instance.Tile.PathFinder.SetObstacle(randomIndex, true);
            tileTestList[randomIndex].SetColor(TileTest.TileTestColorTag.Obstacle);
        }
    }

    private void PathFindingTest()
    {
        int startIndex = 0;
        int destIndex = TileModule.WidthCount * TileModule.HeightCount - 1;

        for (int y = 0; y < TileModule.HeightCount; ++y)
        {
            for (int x = 0; x < TileModule.WidthCount; ++x)
            {
                var index = x + y * TileModule.WidthCount;

                BattleManager.Instance.Tile.PathFinder.SetObstacle(index, false);
                tileTestList[index].SetColor(TileTest.TileTestColorTag.Default);
            }
        }

        if (listObstacle == null || listObstacle.Count == 0)
        {
            CreateRandomObstacle(new List<int>(2) { startIndex, destIndex });
        }
        else
        {
            for (int i = 0; i < listObstacle.Count; ++i)
            {
                BattleManager.Instance.Tile.PathFinder.SetObstacle(listObstacle[i], true);
                tileTestList[listObstacle[i]].SetColor(TileTest.TileTestColorTag.Obstacle);
            }
        }


        var path = BattleManager.Instance.Tile.FindPathImmediately(startIndex, destIndex);

        path?.ForEach(index => tileTestList[index].SetColor(TileTest.TileTestColorTag.Path));
        tileTestList[startIndex].SetColor(TileTest.TileTestColorTag.Start);
        tileTestList[destIndex].SetColor(TileTest.TileTestColorTag.Dest);
    }

    private void PathFindTimeTest()
    {
        int startIndex = 0;
        int destIndex = TileModule.WidthCount * TileModule.HeightCount - 1;

        for (int y = 0; y < TileModule.HeightCount; ++y)
        {
            for (int x = 0; x < TileModule.WidthCount; ++x)
            {
                var index = x + y * TileModule.WidthCount;

                BattleManager.Instance.Tile.PathFinder.SetObstacle(index, false);
                tileTestList[index].SetColor(TileTest.TileTestColorTag.Default);
            }
        }

        var stopWatch = new Stopwatch();
        int curCount = 0;
        long totalTime = 0;
        while (curCount < testCount)
        {
            for (int i = 0; i < TileModule.TotalCount; ++i)
            {
                BattleManager.Instance.Tile.PathFinder.SetObstacle(i, false);
            }
            
            for (int i = 0; i < obstacleCount; ++i)
            {
                int randomIndex = Random.Range(0, TileModule.WidthCount * TileModule.HeightCount);

                if (randomIndex == startIndex || randomIndex == destIndex) continue;

                BattleManager.Instance.Tile.PathFinder.SetObstacle(randomIndex, true);
            }
            
            stopWatch.Start();
            
            var path = BattleManager.Instance.Tile.FindPathImmediately(startIndex, destIndex);
            
            stopWatch.Stop();

            if (path == null) continue;

            ++curCount;
            totalTime += stopWatch.ElapsedMilliseconds;
        }
        
        Debug.Log($"평균 시간 : {totalTime / testCount}ms");
    }

    private void SpawnCharacter()
    {
        Instantiate(prefabCharacter);
    }
}
