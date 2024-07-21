using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDecorator : MonoBehaviour
{
    public GameObject prefab_wobbleDecoration;
    public BoxCollider decorationGeneratorBounds;
    public int numberOfDecorations = 10; // Number of decorations to generate

    // Start is called before the first frame update
    void Start()
    {
        if (decorationGeneratorBounds == null)
        {
            decorationGeneratorBounds = GetComponent<BoxCollider>();
        }
        GenerateDecoration();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateDecoration()
    {
        for (int i = 0; i < numberOfDecorations; i++)
        {
            // Generate random position within the bounds
            Vector3 randomPosition = GetRandomPositionWithinBounds();

            // Generate random rotation
            Quaternion randomRotation = Random.rotation;

            // Generate random scale
            float randomScale = Random.Range(0.5f, 2.0f);
            Vector3 randomSize = new Vector3(randomScale, randomScale, randomScale);

            // Instantiate the prefab at the random position with the random rotation
            GameObject decoration = Instantiate(prefab_wobbleDecoration, randomPosition, randomRotation);

            // Set the random size
            decoration.transform.localScale = randomSize;

            // Make the decoration a child of the SceneDecorator to keep the hierarchy organized
            decoration.transform.parent = this.transform;
        }
        GameObject motherEye = Instantiate(prefab_wobbleDecoration, this.transform);
        motherEye.transform.localScale = new Vector3(8, 8, 8);
    }

    private Vector3 GetRandomPositionWithinBounds()
    {
        Vector3 center = decorationGeneratorBounds.center;
        Vector3 size = decorationGeneratorBounds.size;

        float randomX = Random.Range(-size.x / 2, size.x / 2);
        float randomY = Random.Range(-size.y / 2, size.y / 2);
        float randomZ = Random.Range(-size.z / 2, size.z / 2);

        // Calculate the local random position
        Vector3 localRandomPosition = new Vector3(randomX, randomY, randomZ);

        // Convert the local position to world space position
        return decorationGeneratorBounds.transform.TransformPoint(localRandomPosition);
    }
}
