using UnityEngine;

public class WobbleObject : MonoBehaviour
{
    private float amplitude; // The height of the oscillation
    private float frequency; // The speed of the oscillation
    private float startPositionY; // The starting Y position of the object
    private float offset; // Random offset for the sine wave

    void Start()
    {
        // Initialize random parameters for each object
        amplitude = Random.Range(0.5f, 2.0f); // Random amplitude between 0.5 and 2.0 units
        frequency = Random.Range(0.5f, 2.0f); // Random frequency between 0.5 and 2.0 units
        startPositionY = transform.position.y; // Store the starting Y position
        offset = Random.Range(0f, 2 * Mathf.PI); // Random offset for the sine wave
    }

    void Update()
    {
        // Calculate the new Y position using a sine wave
        float newY = startPositionY + amplitude * Mathf.Sin(Time.time * frequency + offset);

        // Apply the new position to the object
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
