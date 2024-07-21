using UnityEngine;

public class DetailPlacer : MonoBehaviour
{
    public GameObject detailPrefab;
    public Terrain terrain;
    public int numberOfDetails = 100;
    public float minDistanceFromPlayer = 2f;

    void Start()
    {
        PlaceDetails();
    }

    void PlaceDetails()
    {
        for (int i = 0; i < numberOfDetails; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(0, terrain.terrainData.size.x),
                0,
                Random.Range(0, terrain.terrainData.size.z)
            );
            randomPosition.y = terrain.SampleHeight(randomPosition);

            Vector3 detailPosition = terrain.transform.position + randomPosition;

            if (Vector3.Distance(detailPosition, transform.position) > minDistanceFromPlayer)
            {
                Instantiate(detailPrefab, detailPosition, Quaternion.identity);
            }
        }
    }
}
