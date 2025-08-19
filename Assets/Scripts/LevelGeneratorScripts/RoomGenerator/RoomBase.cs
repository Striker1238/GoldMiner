using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomBase : IRoom
{
    public int Seed { get; private set; }
    public int X { get; private set; }
    public int Y { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public TileType[,] RoomData { get; private set; }
    public Dictionary<int,GameObject> SpawnObjects { get; set; } 
    public Vector2Int Center => new Vector2Int(
        X + Width / 2,
        Y + Height / 2);
    public RoomBase(int seed, int x, int y, int width, int height)
    {
        Seed = seed;
        X = x;
        Y = y;
        Width = width;
        Height = height;
        RoomData = new TileType[width, height];
    }
    public virtual Task GenerateRoom()
    {
        Debug.Log($"Generating room at ({X}, {Y}) with size {Width}x{Height}, center: {Center} using seed {Seed}.");

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                RoomData[x,y] = TileType.Floor;
            }
        }

        return Task.CompletedTask;
    }

    public virtual Task DrawRoom(Dictionary<int,TileBase> tiles, Tilemap ground, Tilemap walls)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                ground.SetTile(new Vector3Int(X + x, Y + y, 0), tiles[(int)RoomData[x, y]]);
            }
        }
        return Task.CompletedTask;
    }
}