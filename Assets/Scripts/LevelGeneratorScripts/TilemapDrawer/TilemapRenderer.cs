using UnityEngine.Tilemaps;
using UnityEngine;
using System.Collections;
using System;

public class TilemapRenderer
{
    /// <summary>
    /// Тайлмап карты, который будет использоваться для отрисовки карты.
    /// </summary>
    private Tilemap tilemap;
    /// <summary>
    /// Палитра тайлов для пола
    /// </summary>
    private TilePalette floorTilePalettes;
    /// <summary>
    /// Палитра тайлов для стен
    /// </summary>
    private TilePalette wallTilePalettes;
    /// <summary>
    /// Тайл для границ карты
    /// </summary>
    private TileBase borderTile;
    /// <summary>
    /// Данные карты, которая будет отрисовываться
    /// </summary>
    private Map map;

    public TilemapRenderer(Tilemap tilemap, TilePalette floorTilePalettes, TilePalette wallTilePalettes, TileBase borderTile, Map map)
    {
        this.tilemap = tilemap;
        this.floorTilePalettes = floorTilePalettes;
        this.wallTilePalettes = wallTilePalettes;
        this.borderTile = borderTile;
        this.map = map;
    }

    /// <summary>
    /// Отрисовка карты на тайлмапе.
    /// </summary>
    public void MapDrawing()
    {
        for (int x = 0; x < map.Width; x++)
        {
            for (int y = 0; y < map.Height; y++)
            {
                TileBase tileToSet = null;
                int mask = 0;

                switch (map.Data[x, y])
                {
                    case TileType.Floor:
                        mask = CalculateBitmask(x, y, TileType.Wall);
                        tileToSet = floorTilePalettes.GetTileByBitmask(mask);
                        break;
                    case TileType.Wall:
                        mask = CalculateBitmask(x, y, TileType.Floor);
                        tileToSet = wallTilePalettes.GetTileByBitmask(mask);
                        break;
                    case TileType.Door:
                        break;
                    case TileType.None:
                    default:
                        tileToSet = borderTile;
                        break;
                }


                tilemap.SetTile(new Vector3Int(x, y, 0), tileToSet);
            }
        }
        Debug.Log("Map drawing completed.");

    }
    private int CalculateBitmask(int x, int y, TileType targetType)
    {
        int mask = 0;
        try
        {

            if (map.Data[x, y + 1] == targetType) mask |= 1;
            if (map.Data[x + 1, y + 1] == targetType) mask |= 2;
            if (map.Data[x + 1, y] == targetType) mask |= 4;
            if (map.Data[x + 1, y - 1] == targetType) mask |= 8;
            if (map.Data[x, y - 1] == targetType) mask |= 16;
            if (map.Data[x - 1, y - 1] == targetType) mask |= 32;
            if (map.Data[x - 1, y] == targetType) mask |= 64;
            if (map.Data[x - 1, y + 1] == targetType) mask |= 128;

        }
        catch (Exception ex)
        {
            Debug.LogError($"Error calculating bitmask at ({x}, {y}, {targetType}): {ex.Message}");
        }
        return mask;
    }
}