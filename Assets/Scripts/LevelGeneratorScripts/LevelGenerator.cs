using System.Collections;
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
    [SerializeField] private int lootCount = 5;
    /// <summary>
    /// Словарь, где ключ - количество объектов для спавна, значение - префаб лута
    /// </summary>
    [SerializeField] private List<GameObject> lootSpawnPrefabs;

    private Map map;
    private List<IRoom> rooms;

    public void Start()
    {
        StartLevelGeneration();
    }
    public IEnumerator StartLevelGeneration()
    {
        Debug.Log("Starting level generation...");


        Debug.Log("Clearing existing tiles...");
        tilemap.ClearAllTiles();
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
        TilemapRenderer tilemapRenderer = new(tilemap, floorTilePalettes, wallTilePalettes, borderTile, map);
        tilemapRenderer.MapDrawing();



        LootGenerator lootGenerator = new ();
        lootGenerator.GenerateLoot(map,lootSpawnPrefabs,tilemap);
        yield return null;
    }

    public void StartLevelGenerationEditor()
    {
        StartCoroutine(StartLevelGeneration());
    }
}
