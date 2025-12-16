using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LootGenerator
{
    public void GenerateLoot(Map map, List<LootSpawnData> lootSpawnPrefabs, Tilemap tilemap)
    {
        if (map == null || lootSpawnPrefabs == null || lootSpawnPrefabs.Count == 0)
        {
            Debug.LogWarning("Loot generation failed: Map or loot prefabs are not set.");
            return;
        }
        foreach (var loot in lootSpawnPrefabs)
        {
            var countSpawn = loot.lootCount;

            for (int i = 0; i < countSpawn; i++)
            {
                Vector3Int randomPosition = (Vector3Int)map.GetRandomEmptyPosition();

                if (randomPosition == null) break;

                Vector3 worldPosition = tilemap.GetCellCenterWorld(randomPosition);
                GameObject lootInstance = Object.Instantiate(
                    loot.lootSpawnPrefabs,
                    worldPosition,
                    Quaternion.identity, 
                    tilemap.transform);

            }
        }
        
    }
}