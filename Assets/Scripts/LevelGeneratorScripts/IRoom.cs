using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

public interface IRoom
{
    int Seed { get; }
    int X { get; }
    int Y { get; }
    int Width { get; }
    int Height { get; }
    TileType[,] RoomData { get; }
    Vector2Int Center { get; }
    List<GameObject> SpawnObjects { get; }
    Task DrawRoom(Dictionary<int, TileBase> tiles, Tilemap ground, Tilemap walls);
    Task GenerateRoom();
}