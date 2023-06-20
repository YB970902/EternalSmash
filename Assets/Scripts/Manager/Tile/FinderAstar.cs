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
            int x = Mathf.Abs(Index % TileManager.WidthCount - destX);
            int y = Mathf.Abs(Index / TileManager.WidthCount - destY);
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
        int width = TileManager.WidthCount;
        int height = TileManager.HeightCount;

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

    public void SetObstacle(int index, bool isObstacle)
    {
        if (IsOutOfTile(index)) return;
        tileList[index].IsObstacle = isObstacle;
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

        tileList.ForEach(tile => tile.Reset());
        openList.Clear();

        openList.Enqueue(tileList[startIndex], 0);

        destX = destIndex % TileManager.WidthCount;
        destY = destIndex / TileManager.WidthCount;

        AstarTile curTile = null;

        while (openList.Count > 0)
        {
            curTile = openList.Dequeue();

            // 경로를 찾았다.
            if (curTile.Index == destIndex)
            {
                Debug.Log("Find Path");
                break;
            }

            curTile.IsClose = true;
            curTile.IsOpen = false;

            var nearNode = FindNearTile(curTile);
            nearNode.ForEach(tile => AddToOpenList(tile, curTile));

        }

        // 목적지까지 가는 경로가 없는경우.
        if (curTile.Index != destIndex)
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
    /// 주변 노드를 반환한다.
    /// </summary>
    private List<AstarTile> FindNearTile(AstarTile curTile)
    {
        List<AstarTile> result = new List<AstarTile>(8);

        int curX = curTile.Index % TileManager.WidthCount;
        int curY = curTile.Index / TileManager.WidthCount;

        // 상하좌우 방향을 빠르게 찾기 위한 룩업테이블
        int[] dtX = { -1, 1, 0, 0 };
        int[] dtY = { 0, 0, 1, -1 };

        bool[] dirOpen = { false, false, false, false };

        // 상하좌우 검사부터 한다.
        for (Direct i = Direct.Start + 1; i < Direct.End; ++i)
        {
            int index = (int)i;
            int x = curX + dtX[index];
            int y = curY + dtY[index];

            dirOpen[index] = IsOpenableTile(x, y);
            if (dirOpen[index]) result.Add(tileList[x + y * TileManager.WidthCount]);
        }

        // 대각선 방향을 빠르게 찾기 위한 룩업테이블
        int[] dgX = { -1, 1, -1, 1 };
        int[] dgY = { 1, 1, -1, -1 };
        (int, int)[] dgB = {
            ((int)Direct.Left, (int)Direct.Up),
            ((int)Direct.Right, (int)Direct.Up),
            ((int)Direct.Left, (int)Direct.Down),
            ((int)Direct.Right, (int)Direct.Down)
        };

        // 대각선 검사를 한다.
        for (DiagonalDirect i = DiagonalDirect.Start + 1; i < DiagonalDirect.End; ++i)
        {
            int index = (int)i;
            int x = curX + dgX[index];
            int y = curY + dgY[index];

            if (dirOpen[dgB[index].Item1] &&
                dirOpen[dgB[index].Item2] &&
                IsOpenableTile(x, y)) result.Add(tileList[x + y * TileManager.WidthCount]);
        }

        return result;
    }

    /// <summary>
    /// 타일을 오픈 리스트에 넣는다.
    /// </summary>
    private void AddToOpenList(AstarTile tile, AstarTile parentTile)
    {
        // 닫혀있거나 장애물이면 오픈노드가 될 수 없다. 
        if (tile.IsClose || tile.IsObstacle) return;

        if (tile.IsOpen)
        {
            // 이미 오픈리스트에 들어있는 경우.
            if (tile.IsShortPath(destX, destY, parentTile, IsDiagonal(parentTile, tile)))
            {
                // 현재 경로가 더 짧은 경로라면 값을 갱신한다.
                tile.SetH(destX, destY);
                tile.SetG(parentTile, IsDiagonal(parentTile, tile));
                tile.Parent = parentTile;
                openList.UpdatePriority(tile, tile.F);
            }
        }
        else
        {
            tile.SetH(destX, destY);
            tile.SetG(parentTile, IsDiagonal(parentTile, tile));
            tile.IsOpen = true;
            tile.Parent = parentTile;
            openList.Enqueue(tile, tile.F);
        }
    }

    /// <summary>
    /// 인덱스가 타일 범위 밖인지 여부
    /// </summary>
    private bool IsOutOfTile(int index)
    {
        if (index < 0 || index >= totalCount) return true;
        return false;
    }

    private bool IsOutOfTile(int x, int y)
    {
        if (x < 0 || x >= TileManager.WidthCount ||
            y < 0 || y >= TileManager.HeightCount) return true;
        return false;
    }

    /// <summary>
    /// 오픈리스트에 넣을 수 있는 타일인지 여부
    /// </summary>
    private bool IsOpenableTile(int x, int y)
    {
        if (IsOutOfTile(x, y)) return false;

        var tile = tileList[x + y * TileManager.WidthCount];
        if (tile.IsClose || tile.IsObstacle) return false;
        return true;
    }

    /// <summary>
    /// 두 타일이 대각선에 위치한지 여부
    /// </summary>
    private bool IsDiagonal(AstarTile a, AstarTile b)
    {
        int aX = a.Index % TileManager.WidthCount;
        int aY = a.Index / TileManager.WidthCount;

        int bX = b.Index % TileManager.WidthCount;
        int bY = b.Index / TileManager.WidthCount;

        return aX != bX && aY != bY;
    }
}
