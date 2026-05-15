using System.Collections.Generic;
using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    [Header("Loot Prefabs Pool")]
    public List<GameObject> lootItemsPrefabs; // Drag your Weapon/Ammo prefabs here in Inspector

    [Header("Spawn Points")]
    public List<Transform> spawnPoints; // Create empty GameObjects inside houses and drag them here

    [Header("Spawn Settings")]
    [Range(0f, 100f)] public float spawnChance = 75f; // 75% chance a spot will contain loot

    void Start()
    {
        SpawnLootOnMap();
    }

    void SpawnLootOnMap()
    {
        if (lootItemsPrefabs == null || lootItemsPrefabs.Count == 0)
        {
            Debug.LogWarning("No item prefabs assigned to LootSpawner.");
            return;
        }

        foreach (Transform spot in spawnPoints)
        {
            if (spot == null) continue;

            // Randomly decide if this spot gets loot based on spawnChance
            if (Random.Range(0f, 100f) <= spawnChance)
            {
                // Select a random item from our prefab list
                int randomIndex = Random.Range(0, lootItemsPrefabs.Count);
                GameObject selectedItem = lootItemsPrefabs[randomIndex];

                // Spawn it on the ground slightly hovering so it doesn't clip through the floor
                Vector3 spawnPos = spot.position + new Vector3(0, 0.1f, 0);
                Instantiate(selectedItem, spawnPos, Quaternion.identity);
            }
        }
    }
}
