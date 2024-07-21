using UnityEngine;

public class GrassController : MonoBehaviour
{
    public Terrain terrain;
    public Material grassMaterial;
    
    public float windStrength = 1.0f;
    public float windFrequency = 1.0f;
    public float bladeHeight = 1.0f;
    public float density = 1.0f;

    void Update()
    {
        grassMaterial.SetFloat("_WindStrength", windStrength);
        grassMaterial.SetFloat("_WindFrequency", windFrequency);
        grassMaterial.SetFloat("_BladeHeight", bladeHeight);
        grassMaterial.SetFloat("_Density", density);
    }
}
