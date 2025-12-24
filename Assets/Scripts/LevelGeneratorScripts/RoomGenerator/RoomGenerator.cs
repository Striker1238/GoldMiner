using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEditor;
using UnityEngine;
using System.Threading.Tasks;

public class RoomGenerator
{
    /// <summary>
    /// Сид комнаты
    /// </summary>
    private int seed;
    /// <summary>
    /// Максимальный размер комнаты (ширина, высота)
    /// </summary>
    private Vector2Int roomMaxSize = new(6, 6);
    /// <summary>
    /// Минимальный размер комнаты (ширина, высота)
    /// </summary>
    private Vector2Int roomMinSize = new(3, 3);
    /// <summary>
    /// Максимальная ширина карты
    /// </summary>
    private int mapWidth = 20;
    /// <summary>
    /// Максимальная высота карты
    /// </summary>>
    private int mapHeight = 20;
    /// <summary>
    /// Количество комнат для генерации
    /// </summary>
    private int countRooms = 3;
    /// <summary>
    /// Минимальный отступ между комнатами
    /// </summary>
    private int padding = 2;
    public RoomGenerator(int seed, int mapWidth, int mapHeight, Vector2Int roomMaxSize, Vector2Int roomMinSize, int countRooms, int padding)
    {
        this.seed = seed;
        this.roomMaxSize = roomMaxSize;
        this.roomMinSize = roomMinSize;
        this.mapWidth = mapWidth;
        this.mapHeight = mapHeight;
        this.countRooms = countRooms;
        this.padding = padding;
    }
    public async Task<List<IRoom>> Generate()
    {
        // Список данных по комнатам, которые можно разместить на карте без пересечений
        List<RoomData> roomDataList = GenerateNonOverlappingRoomData();
        List<IRoom> rooms = new (roomDataList.Count);

        foreach (var data in roomDataList)
        {
            IRoom room = new RoomBase(seed, data.Position.x, data.Position.y, data.Size.x, data.Size.y);
            await room.GenerateRoom(); // Отрисовываем пол комнаты, чтобы было ее очертание
            rooms.Add(room);
        }
        Debug.Log($"Generated {rooms.Count} rooms completed!");
        return rooms;
    }

    /// <summary>
    /// Создаем список уникальных точек для комнат, при которых комнаты не будут пересекаться
    /// </summary>
    /// <returns> Список уникальных точек в формате RoomData</returns>
    private List<RoomData> GenerateNonOverlappingRoomData()
    {
        List<RoomData> roomsData = new();
        int attempts = 0;
        int maxAttempts = countRooms * 20; // Попыток на создание всех комнат


        while (roomsData.Count < countRooms && attempts < maxAttempts)
        {
            // Генерируем случайные размеры комнаты
            int roomWidth = Random.Range(roomMinSize.x, roomMaxSize.x+1);
            int roomHeight = Random.Range(roomMinSize.y, roomMaxSize.y+1);

            // Генерируем случайную позицию комнаты
            int x = Random.Range(2, mapWidth - roomWidth - 1);
            int y = Random.Range(2, mapHeight - roomHeight - 1);

            RoomData newRoom = new RoomData
            {
                Position = new Vector2Int(x, y),
                Size = new Vector2Int(roomWidth, roomHeight)
            };

            RectInt newRect = newRoom.ToRectInt(padding); // Конвертим комнату в RectInt с учетом отступа
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