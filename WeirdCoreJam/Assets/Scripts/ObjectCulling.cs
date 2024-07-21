using System.Collections.Generic;
using UnityEngine;

public class ObjectCulling : MonoBehaviour
{
    private Camera mainCamera;
    private Dictionary<Vector2Int, List<Renderer>> gridCells = new Dictionary<Vector2Int, List<Renderer>>();
    public float cullingRadius = 50f; // Radius around the player where objects will not be culled
    public float cellSize = 10f; // Size of each grid cell
    public float cullingInterval = 0.5f; // Time between culling checks
    private float nextCullingTime = 0f;

    void Start()
    {
        mainCamera = Camera.main;
        InitializeGrid();
    }

    void Update()
    {
        if (Time.time >= nextCullingTime)
        {
            CullGridCells();
            nextCullingTime = Time.time + cullingInterval;
        }
    }

    void InitializeGrid()
    {
        GameObject[] rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject rootObject in rootObjects)
        {
            if (rootObject.CompareTag("DONOTCULL"))
            {
                continue;
            }
            AddRenderersToGrid(rootObject);
        }
        Debug.Log("Grid initialization complete. Total cells: " + gridCells.Count);
    }

    void AddRenderersToGrid(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Vector2Int cell = WorldToGridCell(renderer.transform.position);
            if (!gridCells.ContainsKey(cell))
            {
                gridCells[cell] = new List<Renderer>();
            }
            gridCells[cell].Add(renderer);
        }
    }

    Vector2Int WorldToGridCell(Vector3 position)
    {
        return new Vector2Int(
            Mathf.FloorToInt(position.x / cellSize),
            Mathf.FloorToInt(position.z / cellSize)
        );
    }

    void CullGridCells()
    {
        Vector3 playerPosition = transform.position;
        Vector2Int playerCell = WorldToGridCell(playerPosition);
        List<Vector2Int> cellsToCheck = GetNeighboringCells(playerCell);

        foreach (var cell in gridCells)
        {
            if (cellsToCheck.Contains(cell.Key))
            {
                // Cell is within the player's view or radius
                SetCellActive(cell.Value, true);
            }
            else
            {
                // Cell is outside the player's view or radius
                SetCellActive(cell.Value, false);
            }
        }
    }

    void SetCellActive(List<Renderer> renderers, bool active)
    {
        foreach (var renderer in renderers)
        {
            if (renderer != null)
            {
                renderer.gameObject.SetActive(active);
            }
        }
    }

    List<Vector2Int> GetNeighboringCells(Vector2Int centerCell)
    {
        List<Vector2Int> cells = new List<Vector2Int>();

        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                Vector2Int cell = new Vector2Int(centerCell.x + x, centerCell.y + z);

                // Check if the cell is within the culling radius
                Vector3 cellCenter = new Vector3(cell.x * cellSize + cellSize / 2, 0, cell.y * cellSize + cellSize / 2);
                if (IsWithinCullingRadius(cellCenter, transform.position))
                {
                    cells.Add(cell);
                }
                else if (IsCellVisible(cell))
                {
                    cells.Add(cell);
                }
            }
        }

        return cells;
    }

    bool IsWithinCullingRadius(Vector3 cellCenter, Vector3 playerPosition)
    {
        float distance = Vector3.Distance(playerPosition, cellCenter);
        return distance <= cullingRadius;
    }

    bool IsCellVisible(Vector2Int cell)
    {
        Bounds cellBounds = new Bounds(
            new Vector3(cell.x * cellSize + cellSize / 2, 0, cell.y * cellSize + cellSize / 2),
            new Vector3(cellSize, 1000, cellSize)
        );
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        return GeometryUtility.TestPlanesAABB(planes, cellBounds);
    }

    void OnDrawGizmos()
    {
        if (gridCells == null) return;

        Gizmos.color = Color.green;

        foreach (var cell in gridCells.Keys)
        {
            Vector3 cellCenter = new Vector3(cell.x * cellSize + cellSize / 2, 0, cell.y * cellSize + cellSize / 2);
            Gizmos.DrawWireCube(cellCenter, new Vector3(cellSize, 0, cellSize));
        }
    }
}
