using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTest : MonoBehaviour
{
    [SerializeField] Color clrDefault = Color.white;
    [SerializeField] Color clrPath = Color.gray;
    [SerializeField] Color clrStart = Color.red;
    [SerializeField] Color clrDest = Color.blue;
    [SerializeField] Color clrObstacle = Color.black;

    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] float tileSize = 1.0f;

    public enum TileTestColorTag
    {
        Default,
        Path,
        Start,
        Dest,
        Obstacle,
    }

    public int Index { get; private set; }

    private void Awake()
    {
        spriteRenderer.color = clrDefault;
    }

    public void SetTilePosition(int index)
    {
        Index = index;

        Vector2 pos = Vector2.zero;

        pos.x = index % TileModule.WidthCount * tileSize;
        pos.y = index / TileModule.WidthCount * tileSize;

        transform.position = pos;
    }

    public void SetColor(TileTestColorTag tag)
    {
        Color color;

        switch(tag)
        {
            case TileTestColorTag.Default:
                color = clrDefault;
                break;
            case TileTestColorTag.Path:
                color = clrPath;
                break;
            case TileTestColorTag.Start:
                color = clrStart;
                break;
            case TileTestColorTag.Dest:
                color = clrDest;
                break;
            case TileTestColorTag.Obstacle:
                color = clrObstacle;
                break;
            default:
                color = clrDefault;
                break;
        }

        spriteRenderer.color = color;
    }
}
