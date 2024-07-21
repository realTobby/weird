using UnityEngine;
using System.Collections.Generic;

public class GrassRenderer : MonoBehaviour
{
    public GameObject grassPrefab; // The grass mesh prefab
    public int grassCount = 1000; // Number of grass blades to generate
    public float areaSize = 50f; // Size of the area to cover with grass
    public float minHeight = 0.5f; // Minimum height of grass blades
    public float maxHeight = 2f; // Maximum height of grass blades

    private List<GameObject> grassBlades;
    private Camera mainCamera;
    private Plane[] frustumPlanes;

    void Start()
    {
        grassBlades = new List<GameObject>();
        mainCamera = Camera.main;

        // Generate grass blades
        for (int i = 0; i < grassCount; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(-areaSize / 2, areaSize / 2),
                10, // Start raycast from above
                Random.Range(-areaSize / 2, areaSize / 2)
            );

            RaycastHit hit;
            if (Physics.Raycast(randomPosition, Vector3.down, out hit))
            {
                Vector3 position = new Vector3(randomPosition.x, hit.point.y, randomPosition.z);
                GameObject grassBlade = Instantiate(grassPrefab, position, Quaternion.identity, transform);

                // Set a random height
                float randomHeight = Random.Range(minHeight, maxHeight);
                grassBlade.transform.localScale = new Vector3(0.05f, randomHeight * 0.005f, 0.05f);

                grassBlades.Add(grassBlade);
            }
        }

        // Batch rendering setup
        CombineGrassMeshes();
    }

    void Update()
    {
        // Update the frustum planes
        frustumPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

        // Perform frustum culling and occlusion culling
        foreach (var grassBlade in grassBlades)
        {
            grassBlade.SetActive(IsVisible(grassBlade));
        }
    }

    bool IsVisible(GameObject grassBlade)
    {
        // Check if grass blade is within camera frustum
        Bounds bounds = grassBlade.GetComponent<Renderer>().bounds;
        return GeometryUtility.TestPlanesAABB(frustumPlanes, bounds);
    }

    void CombineGrassMeshes()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
            i++;
        }

        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combine);
        gameObject.AddComponent<MeshFilter>().mesh = combinedMesh;
        gameObject.AddComponent<MeshRenderer>().material = grassPrefab.GetComponent<MeshRenderer>().sharedMaterial;

        // Set the parent object active to render combined mesh
        gameObject.SetActive(true);
    }

    void OnDrawGizmos()
    {
        // Draw the area where grass will be generated
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(areaSize, 0, areaSize));
    }
}
