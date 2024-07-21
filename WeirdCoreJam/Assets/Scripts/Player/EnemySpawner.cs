using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> PrefabListEnemies = new List<GameObject>();
    public float Radius = 10f;
    public float SpawnInterval = 3f; // Interval between spawns

    // Start is called before the first frame update
    void Start()
    {
        //// Start the enemy spawning coroutine
        //StartCoroutine(SpawnEnemies());
    }

    // Coroutine to handle enemy spawning
    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(SpawnInterval);
        }
    }

    // Method to spawn a single enemy
    void SpawnEnemy()
    {
        // Select a random enemy prefab from the list
        GameObject enemyPrefab = PrefabListEnemies[Random.Range(0, PrefabListEnemies.Count)];

        // Calculate a random position around the player within the specified radius
        Vector3 spawnPosition = RandomCircle(transform.position, Radius);

        // Instantiate the enemy at the calculated position
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    // Method to calculate a random position around a center point within a radius
    Vector3 RandomCircle(Vector3 center, float radius)
    {
        // Generate a random angle
        float ang = Random.value * 360;
        // Convert the angle to radians
        float angRad = ang * Mathf.Deg2Rad;
        // Calculate the x and y coordinates on the circle
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Cos(angRad);
        pos.y = center.y; // Assuming 2D plane, adjust as needed for 3D
        pos.z = center.z + radius * Mathf.Sin(angRad);
        return pos;
    }
}
