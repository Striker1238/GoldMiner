using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LootGenerator
{
    public void GenerateLoot(Map map, List<GameObject> lootSpawnPrefabs, Tilemap tilemap)
    {
        if (map == null || lootSpawnPrefabs == null || lootSpawnPrefabs.Count == 0)
        {
            Debug.LogWarning("Loot generation failed: Map or loot prefabs are not set.");
            return;
        }
        foreach (var loot in lootSpawnPrefabs)
        {
            for (int i = 0; i < Random.Range(1,3); i++)
            {
                Vector3Int randomPosition = (Vector3Int)map.GetRandomEmptyPosition();

                if (randomPosition == null) break;

                Vector3 worldPosition = tilemap.GetCellCenterWorld(randomPosition);
                GameObject lootInstance = Object.Instantiate(
                    loot,
                    worldPosition,
                    Quaternion.identity);

            }
        }
        
    }
}