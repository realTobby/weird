using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class WaterTime : MonoBehaviour
{
    private Material waterMaterial;

    [Range(2, 256)]
    public int resolution = 10; // Slider in the inspector to control vertex density

    void Start()
    {
        waterMaterial = GetComponent<Renderer>().material;
        CreateMesh();
    }

    void Update()
    {
        float time = Time.time;
        waterMaterial.SetFloat("_CustomTime", time);
    }

    void CreateMesh()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh originalMesh = meshFilter.sharedMesh;
        
        if (originalMesh == null)
        {
            Debug.LogError("No mesh found on the MeshFilter component.");
            return;
        }

        Vector3[] originalVertices = originalMesh.vertices;

        // Calculate the bounds of the original mesh
        Vector3 min = originalVertices[0];
        Vector3 max = originalVertices[0];
        foreach (Vector3 vertex in originalVertices)
        {
            min = Vector3.Min(min, vertex);
            max = Vector3.Max(max, vertex);
        }

        float width = max.x - min.x;
        float height = max.z - min.z;

        Mesh mesh = new Mesh();

        // Create vertices
        Vector3[] vertices = new Vector3[(resolution + 1) * (resolution + 1)];
        for (int y = 0; y <= resolution; y++)
        {
            for (int x = 0; x <= resolution; x++)
            {
                float xPos = min.x + (width * x / resolution);
                float zPos = min.z + (height * y / resolution);
                vertices[y * (resolution + 1) + x] = new Vector3(xPos, min.y, zPos);
            }
        }

        // Create triangles
        int[] triangles = new int[resolution * resolution * 6];
        int triIndex = 0;
        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int baseIndex = y * (resolution + 1) + x;
                triangles[triIndex++] = baseIndex;
                triangles[triIndex++] = baseIndex + resolution + 1;
                triangles[triIndex++] = baseIndex + 1;

                triangles[triIndex++] = baseIndex + 1;
                triangles[triIndex++] = baseIndex + resolution + 1;
                triangles[triIndex++] = baseIndex + resolution + 2;
            }
        }

        // Create UVs
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2((vertices[i].x - min.x) / width, (vertices[i].z - min.z) / height);
        }

        // Assign to mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }
}
