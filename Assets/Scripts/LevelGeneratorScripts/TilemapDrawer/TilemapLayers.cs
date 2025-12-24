using UnityEngine;
using UnityEngine.Tilemaps;

//TODO: думаю стоит реализовать через структуру
[System.Serializable]
public struct TilemapLayers
{
    [SerializeField] private Tilemap floor;
    [SerializeField] private Tilemap walls;
    [SerializeField] private Tilemap loot;


    public TilemapLayers(Tilemap floor, Tilemap walls, Tilemap loot)
    {
        this.floor = floor;
        this.walls = walls;
        this.loot = loot;
    }

    public Tilemap Floor
    {
        get => floor;
        set => floor = value;
    }
    public Tilemap Walls
    {
        get => walls;
        set => walls = value;
    }
    public Tilemap Loot
    {
        get => loot;
        set => loot = value;
    }


    public void ClearAllTiles()
    {
        Floor.ClearAllTiles();
        Walls.ClearAllTiles();
        Loot.ClearAllTiles();

        for (int i = Loot.transform.childCount - 1; i >= 0; i--)
        {
            if (Application.isPlaying)
            {
                GameObject.Destroy(Loot.transform.GetChild(i).gameObject);
            }
            else
            {
                GameObject.DestroyImmediate(Loot.transform.GetChild(i).gameObject);
            }
        }
    }
}