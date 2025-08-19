using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

public interface IRoom
{
    /// <summary>
    /// Сид генерации комнаты.
    /// </summary>
    int Seed { get; }
    /// <summary>
    /// Стартовая позиция комнаты по оси X
    /// </summary>
    int X { get; }
    /// <summary>
    /// Стартовая позиция комнаты по оси Y
    /// </summary>
    int Y { get; }
    /// <summary>
    /// Длина комнаты в клетках
    /// </summary>
    int Width { get; }
    /// <summary>
    /// Высота комнаты в клетках
    /// </summary>
    int Height { get; }
    /// <summary>
    /// Дата о комнате, TileType - тип плитки (пол, стена и т.д.).
    /// </summary>
    TileType[,] RoomData { get; }
    /// <summary>
    /// Метод для получения центра комнаты.
    /// </summary>
    Vector2Int Center { get; }
    Dictionary<int, GameObject> SpawnObjects { get; }
    Task DrawRoom(Dictionary<int, TileBase> tiles, Tilemap ground, Tilemap walls);
    Task GenerateRoom();
   // Task GeneratePositionForObject();
}