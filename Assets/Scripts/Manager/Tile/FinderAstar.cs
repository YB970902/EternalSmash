using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

/// <summary>
/// 에이스타 알고리즘으로 만들어진 길찾기
/// </summary>
public class FinderAstar : IPathFinder
{
    /// <summary>
    /// 타일 정보
    /// </summary>
    public class AstarTile : FastPriorityQueueNode
    {
        /// <summary> 타일의 인덱스 </summary>
        public int Index { get; private set; }

        /// <summary> 목표까지의 대략적인 거리 </summary>
        public int H { get; private set; }
        /// <summary> 지금까지 이동한 거리 </summary>
        public int G { get; private set; }
        /// <summary> 목표까지의 거리 + 이동한 거리 </summary>
        public int F => H + G;

        /// <summary> 오픈 리스트에 있는지 유무 </summary>
        public bool IsOpen { get; set; }
        /// <summary> 클로즈 리스트에 있는지 유무 </summary>
        public bool IsClose { get; set; }

        /// <summary> 장애물인지 여부 </summary>
        public bool IsObstacle { get; set; }

        public AstarTile Parent { get; set; }

        /// <summary> 직선 값 </summary>
        public const int DirectValue = 10;
        /// <summary> 대각선 값 </summary>
        public const int DiagonalValue = 14;

        /// <summary>
        /// 초기화
        /// </summary>
        public void Init(int index)
        {
            Index = index;
            IsObstacle = false;

            Reset();
        }

        /// <summary>
        /// 내용 초기화
        /// </summary>
        public void Reset()
        {
            H = 0;
            G = 0;

            IsOpen = false;
            IsClose = false;

            Parent = null;
        }

        /// <summary>
        /// H값을 계산한다음 대입한다
        /// </summary>
        public void SetH(int destX, int destY)
        {
            H = CalcH(destX, destY);
        }

        /// <summary>
        /// G값을 계산한다음 대입한다
        /// </summary>
        public void SetG(AstarTile prevTile, bool isDiagonal)
        {
            G = CalcG(prevTile, isDiagonal);
        }

        /// <summary>
        /// 현재 경로보다 매개변수로 전달받은 경로가 더 짧은지 여부
        /// </summary>
        public bool IsShortPath(int destX, int destY, AstarTile prevTile, bool isDiagonal)
        {
            int H = CalcH(destX, destY);
            int G = CalcG(prevTile, isDiagonal);

            return F > H + G;
        }

        /// <summary>
        /// H값을 계산한 후 반환한다
        /// </summary>
        private int CalcH(int destX, int destY)
        {
            int x = Mathf.Abs(Index % TileManager.TileXCount - destX);
            int y = Mathf.Abs(Index / TileManager.TileXCount - destY);
            return Mathf.Min(x, y) * DiagonalValue + Mathf.Abs(x - y) * DirectValue;
        }

        /// <summary>
        /// G값을 계산한 후 반환한다
        /// </summary>
        private int CalcG(AstarTile prevTile, bool isDiagonal)
        {
            return isDiagonal ? prevTile.G + DiagonalValue : prevTile.G + DirectValue;
        }
    }

    public enum Direct
    {
        Start = -1,
        Left = 0,
        Right = 1,
        Up = 2,
        Down = 3,
        End = 4,
    }

    public enum DiagonalDirect
    {
        Start = -1,
        LeftUp = 0,
        RightUp = 1,
        LeftDown = 2,
        RightDown = 3,
        End = 4,
    }

    /// <summary> 타일의 총 크기 </summary>
    private int totalCount;

    /// <summary> 타일의 리스트 </summary>
    private List<AstarTile> tileList;

    /// <summary> 오픈 리스트 </summary>
    private FastPriorityQueue<AstarTile> openList;

    /// <summary> 목적지 위치 X </summary>
    private int destX;
    /// <summary> 목적지 위치 Y </summary>
    private int destY;

    public void Init()
    {
        int width = TileManager.TileXCount;
        int height = TileManager.TileYCount;

        totalCount = width * height;

        tileList = new List<AstarTile>(totalCount);
        openList = new FastPriorityQueue<AstarTile>(totalCount);

        for(int y = 0; y < height; ++y)
        {
            for(int x = 0; x < width; ++x)
            {
                var tile = new AstarTile();
                tile.Init(y * width + x);
                tileList.Add(tile);
            }
        }
    }

    public List<int> FindPath(int startIndex, int destIndex)
    {
        // 타일이 범위를 벗어난 경우
        if (IsOutOfTile(startIndex) || IsOutOfTile(destIndex))
        {
            Debug.Log("Index is out of Tile");
            return null;
        }

        List<int> result = new List<int>(totalCount);

        // 이미 목적지에 도착한 경우
        if (startIndex == destIndex)
        {
            Debug.Log("Already at destination");
            return result;
        }

        var curTile = tileList[startIndex];

        int curX = startIndex % TileManager.TileXCount;
        int curY = startIndex / TileManager.TileXCount;

        destX = destIndex % TileManager.TileXCount;
        destY = destIndex / TileManager.TileXCount;

        openList.Enqueue(curTile, 0);

        while (openList.Count > 0)
        {
            SearchNearNode(curTile);
            curTile.IsOpen = false;
            curTile.IsClose = true;
            curTile = openList.Dequeue();

            // 경로를 찾았다.
            if (curTile.Index == destIndex)
            {
                Debug.Log("Find Path");
                break;
            }
        }

        // 목적지까지 가는 경로가 없는경우.
        if (curTile == null)
        {
            Debug.Log("None Path");
            return null;
        }

        while(curTile != null)
        {
            result.Add(curTile.Index);
            curTile = curTile.Parent;
        }

        result.Reverse();

        return result;
    }

    /// <summary>
    /// 주변 노드를 탐색한 후 오픈노드에 넣는다.
    /// </summary>
    private void SearchNearNode(AstarTile curTile)
    {
        int curX = curTile.Index % TileManager.TileXCount;
        int curY = curTile.Index / TileManager.TileXCount;
        // 상하좌우 방향을 빠르게 찾기 위한 룩업테이블
        int[] dtX = { -1, 1, 0, 0 };
        int[] dtY = { 0, 0, 1, -1 };

        bool[] dirOpen = { false, false, false, false };

        for (Direct i = Direct.Start + 1; i < Direct.End; ++i)
        {
            int index = (int)i;
            int x = curX + dtX[index];
            int y = curY + dtY[index];

            dirOpen[index] = IsInTileAndNotClose(x, y);
            if (dirOpen[index]) AddToOpenNode(curTile, x, y);
        }

        // 대각선 방향을 빠르게 찾기 위한 룩업테이블
        int[] dgX = { -1, 1, -1, 1 };
        int[] dgY = { 1, 1, -1, -1 };
        (int, int)[] dgB = { ((int)Direct.Left, (int)Direct.Up), ((int)Direct.Right, (int)Direct.Up), ((int)Direct.Left, (int)Direct.Down), ((int)Direct.Right, (int)Direct.Down) };

        for (DiagonalDirect i = DiagonalDirect.Start + 1; i < DiagonalDirect.End; ++i)
        {
            int index = (int)i;

            int x = curX + dgX[index];
            int y = curY + dgY[index];

            if (dirOpen[dgB[index].Item1] && dirOpen[dgB[index].Item2]) AddToOpenNode(curTile, x, y);
        }
    }

    private void AddToOpenNode(AstarTile curTile, int x, int y)
    {
        var tile = tileList[x + y * TileManager.TileXCount];
        if (tile.IsClose || tile.IsObstacle) return;

        if (tile.IsOpen)
        {
            if (tile.IsShortPath(destX, destY, curTile, IsDiagonal(curTile, tile)))
            {
                // 현재 경로가 더 짧은 경로라면 갱신한다.
                tile.SetH(destX, destY);
                tile.SetG(curTile, IsDiagonal(curTile, tile));
                tile.Parent = curTile;
                openList.UpdatePriority(tile, tile.F);
            }
        }
        else
        {
            tile.SetH(destX, destY);
            tile.SetG(curTile, IsDiagonal(curTile, tile));
            tile.IsOpen = true;
            tile.Parent = curTile;
            openList.Enqueue(tile, tile.F);
        }
    }

    private bool IsOutOfTile(int index)
    {
        if (index < 0 || index >= totalCount) return true;
        return false;
    }

    private bool IsInTileAndNotClose(int x, int y)
    {
        if (x < 0 || x >= TileManager.TileXCount ||
            y < 0 || y >= TileManager.TileYCount) return false;
        if (tileList[x + y * TileManager.TileXCount].IsClose) return false;
        return true;
    }

    private bool IsDiagonal(AstarTile a, AstarTile b)
    {
        int aX = a.Index % TileManager.TileXCount;
        int aY = a.Index / TileManager.TileXCount;

        int bX = b.Index % TileManager.TileXCount;
        int bY = b.Index / TileManager.TileXCount;

        return aX != bX && aY != bY;
    }
}
