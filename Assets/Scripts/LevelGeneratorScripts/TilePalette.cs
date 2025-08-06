using UnityEngine;
using UnityEngine.Tilemaps;
[System.Serializable]
public struct TilePalette
{
    public TileBase topLeftTile, topTile, topRightTile;
    public TileBase leftTile, centerTile, rightTile; 
    public TileBase bottomLeftTile, bottomTile, bottomRightTile;

    public TileBase GetTileByBitmask(int bitmask)
    {
        switch (bitmask)
        {
            case 1: return topTile;
            case 2: return rightTile;
            case 4: return bottomTile;
            case 8: return leftTile;

            case 9: return topLeftTile;
            case 3: return topRightTile;
            case 6: return bottomRightTile;
            case 12: return bottomLeftTile;


            default: return centerTile;
        }
    }
}