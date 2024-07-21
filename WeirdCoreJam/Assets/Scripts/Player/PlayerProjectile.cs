using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public Renderer quadSpriteRenderer;
    public float fadeDuration = 1f; // Duration over which the alpha value will increase
    public SpriteAnimator spriteAnimator;

    private float currentFadeTime;

    // Start is called before the first frame update
    void Start()
    {
        if (spriteAnimator != null)
        {
            // Set the initial alpha value to 0
            spriteAnimator.Init(5,1);
            spriteAnimator.SetAlpha(0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (spriteAnimator != null)
        {
            // Increment the fade timer
            currentFadeTime += Time.deltaTime;

            // Calculate the new alpha value based on the fade duration
            float alpha = Mathf.Clamp01(currentFadeTime / fadeDuration);

            // Set the new alpha value to the sprite animator
            spriteAnimator.SetAlpha(alpha);
        }
    }
}
