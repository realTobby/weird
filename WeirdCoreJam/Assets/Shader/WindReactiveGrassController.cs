using UnityEngine;

public class WindReactiveGrassController : MonoBehaviour
{
    
    public Material grassMaterial;
    public WindZone windZone;
    public Terrain terrain;

    void Update()
    {
        if (grassMaterial != null && windZone != null && terrain != null)
        {
            // Set the wind direction based on WindZone
            Vector3 windDirection = windZone.transform.forward;
            grassMaterial.SetVector("_WindDirection", new Vector4(windDirection.x, windDirection.y, windDirection.z, 0));

            // Set the wind strength based on WindZone main wind
            grassMaterial.SetFloat("_WindStrength", windZone.windMain);

            // Set the wind frequency based on terrain wind settings (wavingGrassStrength as an example)
            grassMaterial.SetFloat("_WindFrequency", terrain.terrainData.wavingGrassStrength * 10.0f);
        }
    }
}
