using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ColorChanger : MonoBehaviour
{
    public Light lightComponent;               // Reference to the Light component
    public GameObject threedeerenderobject;    // Reference to the 3D object to rotate
    public AudioSource audioSource;            // Audio source for background music
    public float colorChangeSpeed = 0.5f;      // Speed for color transition (lower is smoother)
    public float baseLightRange = 10f;         // Base value for light range
    public float rangeWobbleAmount = 2f;       // Maximum wobble offset for light range
    public float wobbleSpeed = 1f;             // Speed of the wobble effect
    public float rotationSpeed = 30f;          // Speed of the 3D object's rotation in degrees per second
    public float audioSensitivity = 2f;        // Sensitivity of crystal's reaction to audio
    public float audioColorInfluence = 0.5f;   // Influence of audio on color transition
    public float audioLightInfluence = 0.5f;   // Influence of audio on light range
    private Color currentColor;                // Track the current color of the light
    private Vector3 targetRotationDirection;   // The target rotation direction
    private float[] audioSamples = new float[64]; // Audio samples for analysis

    void Start()
    {
        // Check if the light component is assigned
        if (lightComponent == null)
        {
            Debug.LogError("No Light component assigned!");
            return;
        }

        // Check if the 3D object is assigned
        if (threedeerenderobject == null)
        {
            Debug.LogError("No 3D Render Object assigned!");
            return;
        }

        // Check if the audio source is assigned
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource component assigned!");
            return;
        }

        // Set the initial color
        currentColor = GenerateRandomColor();
        lightComponent.color = currentColor;

        // Set initial range
        lightComponent.range = baseLightRange;

        // Start the color transition routine
        StartCoroutine(SmoothColorTransitionRoutine());

        // Apply an initial random rotation to the 3D object
        ApplyRandomRotation();

        // Set the initial random rotation direction
        SetRandomRotationDirection();

        // Start a coroutine for continuous rotation
        StartCoroutine(RotateObjectContinuously());
    }

    // Coroutine to handle smooth color transition
    private IEnumerator SmoothColorTransitionRoutine()
    {
        while (true)
        {
            // Generate a truly random new target color
            Color targetColor = GenerateRandomColor();

            // Smoothly transition from the current color to the target color
            yield return StartCoroutine(SmoothChangeColorRoutine(targetColor));
        }
    }

    // Coroutine to change color smoothly to a target color
    private IEnumerator SmoothChangeColorRoutine(Color targetColor)
    {
        float t = 0f;
        Color initialColor = currentColor;

        // Gradually transition between colors using smooth interpolation
        while (t < 1f)
        {
            // Get audio data and influence the effect
            audioSource.GetSpectrumData(audioSamples, 0, FFTWindow.Hamming);
            float audioLevel = GetAverageAudioLevel() * audioSensitivity;

            // Adjust light range based on audio level
            float wobbleOffset = Mathf.Sin(Time.time * wobbleSpeed) * rangeWobbleAmount;
            lightComponent.range = baseLightRange + wobbleOffset + audioLevel * audioLightInfluence;

            // Update transition
            t += Time.deltaTime * (colorChangeSpeed + audioLevel * audioColorInfluence);
            currentColor = Color.Lerp(initialColor, targetColor, t); // Use Lerp for a smoother transition
            lightComponent.color = currentColor; // Assign the color to the Light component

            yield return null; // Wait for the next frame
        }

        // Ensure the target color is set precisely at the end
        currentColor = targetColor;
        lightComponent.color = currentColor;
    }

    // Method to generate a truly random color
    private Color GenerateRandomColor()
    {
        // Generate random values for hue, saturation, and value
        float hue = Random.Range(0f, 1f);
        float saturation = Random.Range(0.7f, 1f); // Higher saturation for more vibrant colors
        float value = Random.Range(0.8f, 1f); // Higher brightness for a well-lit color

        // Convert HSV to RGB and return the new random color
        return Color.HSVToRGB(hue, saturation, value);
    }

    // Method to apply an initial random rotation to the 3D object
    private void ApplyRandomRotation()
    {
        if (threedeerenderobject != null)
        {
            // Set a random rotation on all axes
            float randomX = Random.Range(0f, 360f);
            float randomY = Random.Range(0f, 360f);
            float randomZ = Random.Range(0f, 360f);

            threedeerenderobject.transform.rotation = Quaternion.Euler(randomX, randomY, randomZ);
        }
    }

    // Method to apply continuous rotation to the 3D object
    private IEnumerator RotateObjectContinuously()
    {
        while (true)
        {
            // Get audio data and influence rotation speed
            audioSource.GetSpectrumData(audioSamples, 0, FFTWindow.Hamming);
            float audioLevel = GetAverageAudioLevel() * audioSensitivity;

            // Adjust rotation based on audio level
            float currentRotationSpeed = rotationSpeed + (audioLevel * rotationSpeed);

            // Smoothly interpolate rotation direction
            Vector3 currentRotation = threedeerenderobject.transform.eulerAngles;
            Vector3 targetRotation = currentRotation + (targetRotationDirection * currentRotationSpeed * Time.deltaTime);
            threedeerenderobject.transform.eulerAngles = Vector3.Lerp(currentRotation, targetRotation, Time.deltaTime * currentRotationSpeed);

            // Randomly change the direction over time
            if (Random.value < 0.01f) // 1% chance every frame to change direction
            {
                SetRandomRotationDirection();
            }

            yield return null;
        }
    }

    // Method to set a random rotation direction
    private void SetRandomRotationDirection()
    {
        targetRotationDirection = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized;
    }

    // Method to calculate the average audio level
    private float GetAverageAudioLevel()
    {
        float sum = 0f;
        foreach (float sample in audioSamples)
        {
            sum += sample;
        }
        return sum / audioSamples.Length;
    }
}
