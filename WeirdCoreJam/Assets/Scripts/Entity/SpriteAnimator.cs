using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    public int columns = 0; // Number of columns in the spritesheet
    public int rows = 0;    // Number of rows in the spritesheet
    public float framesPerSecond = 10f; // Speed of the animation

    private Renderer quadRenderer;
    private Material quadMaterial;
    private Vector2 textureSize;
    private int currentFrame;
    private float frameDuration;
    private float timer;
    private Color currentColor;

    void Start()
    {
        quadRenderer = GetComponent<Renderer>();
        if (quadRenderer != null)
        {
            quadMaterial = quadRenderer.material;
            if (quadMaterial.HasProperty("_Color"))
            {
                currentColor = quadMaterial.color; // Initialize current color
            }
        }
    }

    public void SetMaterial(Material mat)
    {
        quadRenderer = GetComponent<Renderer>();
        quadRenderer.material = mat;
        quadMaterial = mat;
        if (quadMaterial.HasProperty("_Color"))
        {
            currentColor = quadMaterial.color; // Initialize current color
        }
    }

    public void Init(int spriteCol, int spriteRow)
    {
        columns = spriteCol;
        rows = spriteRow;

        quadRenderer = GetComponent<Renderer>();
        quadMaterial = quadRenderer.material;

        // Calculate the size of each sprite in the spritesheet
        textureSize = new Vector2(1f / spriteCol, 1f / spriteRow);

        // Set the material's initial scale
        quadMaterial.SetTextureScale("_MainTex", textureSize);

        frameDuration = 1f / framesPerSecond;
    }

    public void SetAlpha(float alpha)
    {
        if (quadMaterial != null && quadMaterial.HasProperty("_Color"))
        {
            Color color = quadMaterial.color;
            color.a = alpha;
            quadMaterial.color = color;
        }
    }

    void Update()
    {
        if (quadMaterial == null)
            return;

        // Update the timer
        timer += Time.deltaTime;

        if(columns == 0 && rows == 0)
        {
            return;
        }

        // If it's time to change the frame
        if (timer >= frameDuration)
        {
            timer -= frameDuration;

            // Calculate the next frame index
            currentFrame = (currentFrame + 1) % (columns * rows);

            // Calculate the offset for the current frame
            Vector2 offset = new Vector2((currentFrame % columns) * textureSize.x, 1f - ((currentFrame / columns) * textureSize.y) - textureSize.y);

            // Apply the offset to the material
            quadMaterial.SetTextureOffset("_MainTex", offset);
        }
    }
}
