using UnityEngine;

public class LightController : MonoBehaviour
{
    public Light pointLight;
    public float rangeSpeed = 1.0f;
    public float intensitySpeed = 1.0f;
    public Color[] colors;
    public float colorChangeSpeed = 1.0f;

    private float rangeTarget;
    private float intensityTarget;
    private int currentColorIndex;
    private float colorLerpTime;

    void Start()
    {
        if (pointLight == null)
        {
            pointLight = GetComponent<Light>();
        }
        rangeTarget = pointLight.range;
        intensityTarget = pointLight.intensity;
        currentColorIndex = 0;
        colorLerpTime = 0.0f;
    }

    void Update()
    {
        // Create a pulsating range effect
        rangeTarget = Mathf.PingPong(Time.time * rangeSpeed, 100.0f);
        pointLight.range = rangeTarget + 75;

        // Create a pulsating intensity effect
        intensityTarget = Mathf.PingPong(Time.time * intensitySpeed, 1.0f);
        pointLight.intensity = intensityTarget;

        // Change color over time
        if (colors.Length > 0)
        {
            colorLerpTime += Time.deltaTime * colorChangeSpeed;
            if (colorLerpTime > 1.0f)
            {
                colorLerpTime = 0.0f;
                currentColorIndex = (currentColorIndex + 1) % colors.Length;
            }

            pointLight.color = Color.Lerp(pointLight.color, colors[currentColorIndex], colorLerpTime);
        }
    }
}
