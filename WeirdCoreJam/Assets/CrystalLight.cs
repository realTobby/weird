using UnityEngine;

public class CrystalLight : MonoBehaviour
{
    public Light pointLight;
    public Renderer objectRenderer;

    private void Start()
    {
        // Set a random rotation for the 3D object
        transform.rotation = Random.rotation;

        // Generate a random color
        Color randomColor = new Color(Random.value, Random.value, Random.value);

        // Assign the random color to the point light
        pointLight.color = randomColor;

        // Assign the random color to the object's material and enable emission
        Material material = objectRenderer.material;
        material.color = randomColor;
        material.SetColor("_EmissionColor", randomColor);
        material.EnableKeyword("_EMISSION");
    }

    private void Update()
    {
        // Continuously rotate the 3D object
        transform.Rotate(new Vector3(Random.value, Random.value, Random.value) * Time.deltaTime * 50);
    }
}
