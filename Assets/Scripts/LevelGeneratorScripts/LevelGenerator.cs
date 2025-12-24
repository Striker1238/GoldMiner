using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Основной класс генерации уровня, который отвечает за генерацию карты, комнат, коридоров и отрисовку всего этого на тайлмапе.
/// Данный скрипт размещается на сцене Unity и запускает процесс генерации уровня
/// </summary>
public class LevelGenerator : MonoBehaviour
{
    public TilemapLayers tilemapLayers;

    public TilePalette floorTilePalettes;
    public TilePalette wallTilePalettes;
    public TileBase borderTile;

    
    [Header("Generator map settings")]
    [SerializeField] private int width = 20;
    [SerializeField] private int height = 20;
    [SerializeField] private int countRooms = 3;
    [SerializeField] private int padding = 2;
    [SerializeField] private int corridorWidth = 1;
    [SerializeField] private Vector2Int roomMaxSize = new(6,6);
    [SerializeField] private Vector2Int roomMinSize = new(3,3);

    [Header("Loot generator settings")]
    /// <summary>
    /// Лист создаваемых предметов на карте в виде структуры с полями количество и объект
    /// </summary>
    [SerializeField] private List<LootSpawnData> lootSpawnPrefabs;


    private Map map;
    private List<IRoom> rooms;

    // перенести все генераторы в приватные поля, дабы не было создания каждый раз, когда создается карта
    //private RoomGenerator roomGenerator;// TODO: добавить метод для обновления параметров о комнатах и мапе в целом

    //TODO Добавить тесты для генерации карты

    public LevelGenerator()
    {

    }



    public void Start()
    {
        StartLevelGeneration();
    }
    public IEnumerator StartLevelGeneration()
    {
        Debug.Log("Starting level generation...");


        Debug.Log("Clearing existing tiles...");
        tilemapLayers.ClearAllTiles();
        map = null;
        rooms = null;


        Debug.Log("Generating new seed...");
        SeedGenerator.RegenerateSeed();


        map = new Map(width, height, SeedGenerator.Seed);
        rooms = new List<IRoom>(countRooms);

        // Генерация комнат
        Debug.Log("Generating rooms...");
        RoomGenerator roomGenerator = new RoomGenerator(
            SeedGenerator.Seed,
            width,
            height,
            roomMaxSize,
            roomMinSize,
            countRooms,
            padding
        );
        rooms.AddRange(roomGenerator.Generate().Result);
        // Установка тайлов от комнат на карту
        foreach (var room in rooms)
            map.SetTileType(room);

        Debug.Log("Generating corridors...");
        CorridorGenerator corridorGenerator = new (corridorWidth);
        for (int i = 0; i < rooms.Count - 1; i++)
        {
            var start = rooms[i].Center;
            var end = rooms[i + 1].Center;
            corridorGenerator.GenerateNoisyCorridor(start, end, ref map);
        }

        Debug.Log("Generating outline...");
        map.PerimetrOutline();


        Debug.Log("Drawing map...");
        TilemapRenderer tilemapRenderer = new(tilemapLayers, floorTilePalettes, wallTilePalettes, borderTile, map);
        tilemapRenderer.MapDrawing();



        LootGenerator lootGenerator = new ();
        lootGenerator.GenerateLoot(map,lootSpawnPrefabs, tilemapLayers.Loot);
        yield return null;
    }

#if UNITY_EDITOR
    public void StartLevelGenerationEditor()
    {
        StartCoroutine(StartLevelGeneration());
    }
#endif
}
