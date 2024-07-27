using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class introscene : MonoBehaviour
{

    // intro animation: fade text in, play sound, fade out, transition

    public SpriteRenderer uiText;  // Reference to the UI Text component
    public float fadeDuration = 1f;  // Duration for the fade-in effect
    public float displayDuration = 1f;  // Time to display the text before fading out
    public AudioSource audioSource;  // Reference to an AudioSource for playing sound
    public AudioClip fadeInSound;  // Sound to play when text fades in
    public GameObject uiSpriteTextImage;  // The GameObject for the UI sprite image


    // Start is called before the first frame update
    void Start()
    {
        // fade in with coroutine, then play sound

        StartCoroutine(nameof(FadeTextIn));


    }

    private IEnumerator FadeTextIn()
    {
        Color originalColor = uiText.color;
        originalColor.a = 0f;
        uiText.color = originalColor;

        float timer = 0f;

        // Fade in
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            uiText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;  // Wait for the next frame
        }

        uiText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);

        // Play sound
        if (audioSource != null && fadeInSound != null)
        {
            audioSource.PlayOneShot(fadeInSound);
        }

        // Display the text for a while
        yield return new WaitForSeconds(displayDuration);

        // Fade out
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            uiText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;  // Wait for the next frame
        }

        uiText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        // Optionally, transition to another scene or perform another action
        OnIntroEnd();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnIntroEnd()
    {
        // transition to first level
        SceneManager.LoadScene(1);
    }

}
