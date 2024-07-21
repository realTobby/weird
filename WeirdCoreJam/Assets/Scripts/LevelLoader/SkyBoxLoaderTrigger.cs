using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxLoaderTrigger : MonoBehaviour
{
    public Material skyboxMaterial; // Updated to use Material instead of Skybox

    // Start is called before the first frame update
    void Start()
    {
        // Initialize if needed
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
            // Change skybox
            RenderSettings.skybox = skyboxMaterial;
        }
    }
}
