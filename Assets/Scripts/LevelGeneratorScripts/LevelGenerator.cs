
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    [Header("Tiles")]
    public Tilemap tilemap;
    public TileBase floorTile;
    public TileBase wallTile;
    public TileBase borderTile;

    
    [Header("Generator settings")]
    [SerializeField] private int width = 20;
    [SerializeField] private int height = 20;
    [SerializeField] private int countRooms = 3;
    [SerializeField] private int padding = 2;
    [SerializeField] private int corridorWidth = 1;
    [SerializeField] private Vector2Int roomMaxSize = new(6,6);
    [SerializeField] private Vector2Int roomMinSize = new(3,3);

    private int seed;
    private TileType[,] mapData;

    private List<IRoom> rooms = new List<IRoom>();

    public void Start()
    {
        StartLevelGeneration();
    }
    public void StartLevelGeneration()
    {

        Debug.Log("Starting level generation...");

        Debug.Log("Clearing existing tiles...");
        tilemap.ClearAllTiles();
        rooms.Clear();
        mapData = null;

        Debug.Log("Generating new seed...");
        seed = SeedGenerator.GenerateSeed();
        Random.InitState(seed);
        InitializedMapData();

        //BorderDrawing();

        Generate();
    }
    private async Task Generate()
    {
        // 1. Генерируем данные попозиции и размеру для комнат
        List<RoomData> roomDataList = GenerateNonOverlappingRoomData();

        // 2. Создаем комнаты на основе этих данных
        foreach (var data in roomDataList)
        {
            IRoom room = new RoomBase(seed, data.Position.x, data.Position.y, data.Size.x, data.Size.y);
            await room.GenerateRoom();

            for (int x = 0; x < room.Width; x++)
            {
                for (int y = 0; y < room.Height; y++)
                {
                    int mx = data.Position.x + x;
                    int my = data.Position.y + y;
                    if (mx >= 0 && mx < mapData.GetLength(0) && my >= 0 && my < mapData.GetLength(1))
                    {
                        mapData[mx, my] = room.RoomData[x, y];
                    }
                }
            }
            rooms.Add(room);
        }

        // 3. К каждой комнате создаем маршрут, который будет соединять комнаты
        await ConnectingRoom();

        // 4. Все комнаты обводим стенами
        await PerimetrOutline();

        //  Рисуем карту
        await MapDrawing();
    }

    private async Task ConnectingRoom()
    {
        Debug.Log("Connecting rooms...");
        for (int i = 0; i < rooms.Count - 1; i++)
        {
            var start = rooms[i].Center;
            var end = rooms[i + 1].Center;
            GenerateNoisyCorridor(start, end);
        }
        await Task.CompletedTask;
    }
    private async Task PerimetrOutline()
    {
        Debug.Log("Drawing perimeter outline...");
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Проверяем, есть ли вокруг клетки пол, если есть ставим стену
                if (mapData[x, y] == TileType.None)
                {
                    bool hasFloor = false;
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            if (dx == 0 && dy == 0) continue;
                            int nx = x + dx;
                            int ny = y + dy;
                            if (nx >= 0 && nx < width && ny >= 0 && ny < height && mapData[nx, ny] == TileType.Floor)
                            {
                                hasFloor = true;
                                break;
                            }
                        }
                        if (hasFloor) break;
                    }
                    if (hasFloor)
                    {
                        mapData[x, y] = TileType.Wall;
                    }
                }

            }
        }
        await Task.CompletedTask;
    }

    /*
    private async Task BorderDrawing()
    {
        Debug.Log("Drawing border...");
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), borderTile);
                }
            }
        }
        await Task.CompletedTask;
    }
    */

    private async Task MapDrawing()
    {
        Debug.Log("Drawing map...");
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                TileBase tileToSet = null;
                switch (mapData[x, y])
                {
                    case TileType.Floor:
                        tileToSet = floorTile;
                        break;
                    case TileType.Wall:
                        tileToSet = wallTile;
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
        await Task.CompletedTask;
    }




    private void InitializedMapData()
    {
        mapData = new TileType[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                mapData[x, y] = TileType.None;
            }
        }
    }

    /// <summary>
    /// Генерирует коридор между двумя точками с небольшим шумом.
    /// </summary>
    /// <param name="start">Начальная позиция</param>
    /// <param name="end">Конечная позиция</param>
    private void GenerateNoisyCorridor(Vector2Int start, Vector2Int end)
    {
        Debug.Log($"Generating noisy corridor from {start} to {end}");
        Vector2Int current = start;

        while (current != end)
        {

            if (Random.value < 0.5f)
            {
                current.x += (end.x > current.x) ? 1 : (end.x < current.x ? -1 : 0);
            }
            else
            {
                current.y += (end.y > current.y) ? 1 : (end.y < current.y ? -1 : 0);
            }

            int halfWidth = corridorWidth / 2;
            for (int dx = -halfWidth; dx <= halfWidth; dx++)
            {
                for (int dy = -halfWidth; dy <= halfWidth; dy++)
                {
                    int nx = current.x + dx;
                    int ny = current.y + dy;
                    if (mapData != null && nx >= 0 && nx < mapData.GetLength(0) && ny >= 0 && ny < mapData.GetLength(1))
                        mapData[nx, ny] = TileType.Floor;
                }
            }
        }
    }
    /// <summary>
    /// Создаем список уникальных точек для комнат.
    /// </summary>
    /// <returns> Список уникальных точек</returns>
    private List<RoomData> GenerateNonOverlappingRoomData()
    {
        List<RoomData> roomsData = new();
        int attempts = 0;
        int maxAttempts = countRooms * 20;

        while (roomsData.Count < countRooms && attempts < maxAttempts)
        {
            int roomWidth = Random.Range(roomMinSize.x, roomMaxSize.x);
            int roomHeight = Random.Range(roomMinSize.y, roomMaxSize.y);

            int x = Random.Range(1, width - roomWidth - 1);
            int y = Random.Range(1, height - roomHeight - 1);

            RoomData newRoom = new RoomData
            {
                Position = new Vector2Int(x, y),
                Size = new Vector2Int(roomWidth, roomHeight)
            };

            RectInt newRect = newRoom.ToRectInt(padding);
            bool overlaps = false;

            foreach (var existing in roomsData)
            {
                if (newRect.Overlaps(existing.ToRectInt(padding)))
                {
                    overlaps = true;
                    break;
                }
            }

            if (!overlaps)
            {
                roomsData.Add(newRoom);
            }

            attempts++;
        }

        return roomsData;
    }
    private struct RoomData
    {
        public Vector2Int Position;
        public Vector2Int Size;

        public RectInt ToRectInt(int padding)
        {
            return new RectInt(Position.x - padding, Position.y - padding, Size.x + padding * 2, Size.y + padding * 2);
        }
    }
}
