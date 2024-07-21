using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleSpawner : MonoBehaviour
{
    public GameObject whalePrefab; // Assign the whale prefab in the inspector
    public int numberOfWhales = 5; // Number of whales to spawn per group
    public float spawnInterval = 10f; // Time between spawns
    public float minWhaleSize = 0.5f; // Minimum size of the whales
    public float maxWhaleSize = 2f; // Maximum size of the whales
    public Vector3 spawnAreaCenter = new Vector3(-50, 50, 0); // Center of the area where whales will spawn
    public Vector3 spawnAreaSize = new Vector3(0, 10, 300); // Size of the area where whales will spawn
    public Vector3 targetAreaCenter = new Vector3(50, 50, 0); // Center of the area where whales will move to
    public Vector3 targetAreaSize = new Vector3(0, 10, 300); // Size of the area where whales will move to
    public float whaleSpeed = 2f; // Speed at which whales move
    public int preWarmCount = 10; // Number of whales to pre-warm

    private void Start()
    {
        PreWarm();
        StartCoroutine(SpawnWhaleGroups());
    }

    private void PreWarm()
    {
        for (int i = 0; i < preWarmCount; i++)
        {
            SpawnSingleWhale();
        }
    }

    private IEnumerator SpawnWhaleGroups()
    {
        while (true)
        {
            SpawnWhaleGroup();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnWhaleGroup()
    {
        for (int i = 0; i < numberOfWhales; i++)
        {
            SpawnSingleWhale();
        }
    }

    private void SpawnSingleWhale()
    {
        Vector3 spawnPosition = GetRandomPositionInArea(spawnAreaCenter, spawnAreaSize);
        Vector3 targetPosition = GetRandomPositionInArea(targetAreaCenter, targetAreaSize);
        GameObject whale = Instantiate(whalePrefab, spawnPosition, Quaternion.identity);

        // Calculate direction and set rotation
        Vector3 direction = (targetPosition - spawnPosition).normalized;
        whale.transform.rotation = Quaternion.LookRotation(direction);

        float size = Random.Range(minWhaleSize, maxWhaleSize);
        whale.transform.localScale = new Vector3(size, size, size);
        whale.GetComponent<WhaleMover>().Initialize(targetPosition, whaleSpeed);
    }

    private Vector3 GetRandomPositionInArea(Vector3 center, Vector3 size)
    {
        return center + new Vector3(
            Random.Range(-size.x / 2, size.x / 2),
            Random.Range(-size.y / 2, size.y / 2),
            Random.Range(-size.z / 2, size.z / 2)
        );
    }
}
