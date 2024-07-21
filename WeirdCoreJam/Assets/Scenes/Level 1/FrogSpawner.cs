using System.Collections;
using UnityEngine;

public class FrogSpawner : MonoBehaviour
{
    public GameObject FrogPrefab;
    public Transform PlayerTransform;
    public float spawnInterval = 2f;
    public float minSpawnDistance = 5f;
    public float maxSpawnDistance = 10f;
    public float spawnAngleOffset = 30f;

    void Start()
    {
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        
    }

    IEnumerator SpawnFrogs()
    {
        while (true && CanSpawnFrogs)
        {
            SpawnFrog();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnFrog()
    {
        // Calculate random angle within the spawn area outside player's view
        float randomAngle = Random.Range(-spawnAngleOffset, spawnAngleOffset);
        Quaternion spawnRotation = Quaternion.Euler(0, randomAngle, 0) * PlayerTransform.rotation;
        
        // Calculate random distance within the defined range
        float randomDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
        
        // Calculate spawn position based on the random rotation and distance
        Vector3 spawnPosition = PlayerTransform.position - spawnRotation * Vector3.forward * randomDistance;
        
        // Instantiate the frog at the calculated position
        GameObject newFrog = Instantiate(FrogPrefab, spawnPosition, Quaternion.identity);
        
        // Make the frog look in the same direction the player is facing
        newFrog.transform.LookAt(newFrog.transform.position + PlayerTransform.forward);

        Destroy(newFrog, 6f);
    }


    public bool CanSpawnFrogs = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            CanSpawnFrogs = true;
            StartCoroutine(SpawnFrogs());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            CanSpawnFrogs = false;
        }
    }



}
