using UnityEngine;

public class ProceduralGrassGenerator : MonoBehaviour
{
    public Terrain terrain;
    public GameObject player;
    public GameObject grassPrefab;
    public float grassRadius = 50f; // Radius around the player to generate grass
    public float grassDensity = 0.5f; // Adjust the density of the grass

    private TerrainData terrainData;

    void Start()
    {
        terrainData = terrain.terrainData;

        player = GameObject.FindGameObjectWithTag("Player");


    }

    void Update()
    {
        GenerateGrassAroundPlayer();
    }

    void GenerateGrassAroundPlayer()
    {
        Vector3 playerPosition = player.transform.position;

        // Clear existing details
        int detailIndex = 0; // Assuming the first detail prototype is the grass
        int[,] details = new int[terrainData.detailWidth, terrainData.detailHeight];
        terrainData.SetDetailLayer(0, 0, detailIndex, details);

        // Calculate terrain coordinates based on player position
        Vector3 terrainPosition = playerPosition - terrain.transform.position;
        float terrainSizeX = terrainData.size.x;
        float terrainSizeZ = terrainData.size.z;

        int detailWidth = terrainData.detailWidth;
        int detailHeight = terrainData.detailHeight;

        float scaleX = (float)detailWidth / terrainSizeX;
        float scaleZ = (float)detailHeight / terrainSizeZ;

        int playerDetailX = Mathf.RoundToInt(terrainPosition.x * scaleX);
        int playerDetailZ = Mathf.RoundToInt(terrainPosition.z * scaleZ);

        int radiusInDetailUnits = Mathf.RoundToInt(grassRadius * scaleX);

        // Populate grass around the player
        for (int x = playerDetailX - radiusInDetailUnits; x <= playerDetailX + radiusInDetailUnits; x++)
        {
            for (int z = playerDetailZ - radiusInDetailUnits; z <= playerDetailZ + radiusInDetailUnits; z++)
            {
                if (x >= 0 && x < detailWidth && z >= 0 && z < detailHeight)
                {
                    float distance = Vector2.Distance(new Vector2(playerDetailX, playerDetailZ), new Vector2(x, z));
                    if (distance < radiusInDetailUnits)
                    {
                        details[x, z] = Mathf.FloorToInt((1 - (distance / radiusInDetailUnits)) * grassDensity * 16);
                    }
                }
            }
        }

        terrainData.SetDetailLayer(0, 0, detailIndex, details);
    }
}
