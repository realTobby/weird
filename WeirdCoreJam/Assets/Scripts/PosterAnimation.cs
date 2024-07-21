using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosterAnimation : MonoBehaviour
{
    public Texture2D lowTexture;
    public Texture2D middleTexture;
    public Texture2D highTexture;

    public float switchInterval = 1.0f; // Time in seconds to switch textures

    private Material posterMaterial;
    private float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Get the material attached to the poster
        posterMaterial = GetComponent<Renderer>().material;

        // Set the initial texture
        posterMaterial.mainTexture = lowTexture;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the timer
        timer += Time.deltaTime;

        // Check if it's time to switch the texture
        if (timer >= switchInterval)
        {
            // Reset the timer
            timer = 0.0f;

            // Change the texture
            SwitchTexture();
        }
    }

    void SwitchTexture()
    {
        // Cycle through the textures
        if (posterMaterial.mainTexture == lowTexture)
        {
            posterMaterial.mainTexture = middleTexture;
        }
        else if (posterMaterial.mainTexture == middleTexture)
        {
            posterMaterial.mainTexture = highTexture;
        }
        else
        {
            posterMaterial.mainTexture = lowTexture;
        }
    }
}
