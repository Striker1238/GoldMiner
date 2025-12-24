using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Tilemaps;


/// <summary>
/// Единая карта, которая генерируется путем создания комнат и коридоров, а затем отрисовывается на тайлмапе.
/// </summary>
public class Map
{
    public int Seed { get; private set; }
    /// <summary>
    /// Ширина карты в клетках
    /// </summary>
    public int Width { get; private set; }
    /// <summary>
    /// Высота карты в клетках
    /// </summary>
    public int Height { get; private set; }
    /// <summary>
    /// Данные о карте, TileType - тип клетки (пол, стена и т.д.).
    /// </summary>
    public TileType[,] Data { get; private set; }
    public Map(int width, int height, int seed)
    {
        Seed = seed;
        Width = width;
        Height = height;
        InitializedMapData();
    }
    private List<Vector2Int> lootPosition = new List<Vector2Int>();


    /// <summary>
    /// Инициализация базовых данных карты. Установка всех значений в TileType.None.
    /// </summary>
    private void InitializedMapData()
    {
        Data = new TileType[Width, Height];
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                SetTileType(x,y,TileType.None);
            }
        }
    }

    /// <summary>
    /// Установка типа клетки в заданных координатах на карте размером Width x Height.
    /// </summary>
    /// <param name="x">Позиция клетки на карте по X</param>
    /// <param name="y">Позиция клетки на карте по Y</param>
    /// <param name="type">Значение клетки</param>
    public void SetTileType(int x, int y, TileType type) => Data[x, y] = type;

    /// <summary>
    /// Установка типа клетки в заданных координатах на карте размером Width x Height, используя данные IRoom.
    /// </summary>
    /// <param name="room">Данные о размещаемой комнате</param>
    public void SetTileType(IRoom room)
    {
        for (int x = 0; x < room.Width; x++)
        {
            for (int y = 0; y < room.Height; y++)
            {
                // Вычисление глобальных координат комнаты на карте
                int mx = room.X + x;
                int my = room.Y + y;
                if (mx >= 0 && mx < Width && my >= 0 && my < Height)
                {
                    SetTileType(mx, my, room.RoomData[x, y]);
                }
            }
        }
    }

    /// <summary>
    /// Генерация периметра карты, обрамление стенами всех клеток, которые являются полом.
    /// </summary>
    public void PerimetrOutline()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (Data[x, y] == TileType.None)
                {
                    bool hasFloor = false;
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            if (dx == 0 && dy == 0) continue;
                            int nx = x + dx;
                            int ny = y + dy;
                            if (nx >= 0 && nx < Width && ny >= 0 && ny < Height && Data[nx, ny] == TileType.Floor)
                            {
                                hasFloor = true;
                                break;
                            }
                        }
                        if (hasFloor) break;
                    }
                    if (hasFloor)
                    {
                        SetTileType(x, y, TileType.Wall);
                    }
                }
            }
        }
        Debug.Log("Perimeter outline completed.");
    }

    /// <summary>
    /// Возвращает случайную пустую позицию на карте для размещения лута.
    /// </summary>
    /// <returns></returns>
    public Vector2Int GetRandomEmptyPosition()
    {
        List<Vector2Int> emptyPositions = new List<Vector2Int>();
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (Data[x, y] == TileType.Floor && !lootPosition.Contains(new(x,y)))
                    emptyPositions.Add(new Vector2Int(x, y));
            }
        }
        if (emptyPositions.Count == 0)
        {
            Debug.LogWarning("No empty positions found on the map.");
            return Vector2Int.zero;
        }

        Vector2Int randomPosition = emptyPositions[Random.Range(0, emptyPositions.Count)];
        lootPosition.Add(randomPosition);
        return randomPosition;

    }
}