using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassFieldsInit : MonoBehaviour
{
    public GameObject ParticleSystemFireflies;
    public Color fogColorEnter = Color.grey; // Color of the fog when entering the trigger
    public float fogDensityEnter = 0.02f; // Density of the fog when entering the trigger
    public Color fogColorExit = Color.clear; // Color of the fog when exiting the trigger
    public float fogDensityExit = 0.0f; // Density of the fog when exiting the trigger

    // Start is called before the first frame update
    void Start()
    {
        // Initialize if needed
        ParticleSystemFireflies.SetActive(false); // Ensure fireflies are initially inactive
    }

    // Update is called once per frame
    void Update()
    {
        // Update logic if needed
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Set fog
            RenderSettings.fog = true;
            RenderSettings.fogColor = fogColorEnter;
            RenderSettings.fogDensity = fogDensityEnter;

            // Activate fireflies
            ParticleSystemFireflies.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Set fog
            RenderSettings.fogColor = fogColorExit;
            RenderSettings.fogDensity = fogDensityExit;

            // Deactivate fireflies
            ParticleSystemFireflies.SetActive(false);
        }
    }
}
