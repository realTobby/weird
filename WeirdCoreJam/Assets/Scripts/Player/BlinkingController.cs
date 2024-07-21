using UnityEngine;

public class BlinkingController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private static readonly string EyesOpen = "eyesOpen";
    private static readonly string BlinkingOpen = "blinkingOpen";
    private static readonly string BlinkingClosed = "blinkingClosed";
    private static readonly string EyesClosed = "eyesClosed";

    void Start()
    {
        // Assuming 'animator' is set in the Inspector or through code
        if (animator == null)
        {
            Debug.LogError("Animator reference not set in AnimationController.");
            enabled = false; // Disable the script if Animator is not set
        }
        else
        {
            PlayBlinkingOpen();
        }
    }

    public void PlayBlinkingOpen()
    {
        if (animator != null)
        {
            animator.Play(BlinkingOpen);
        }
    }

    public void PlayBlinkingClosed()
    {
        if (animator != null)
        {
            animator.Play(BlinkingClosed);
        }
    }

    public void PlayEyesClosed()
    {
        if (animator != null)
        {
            animator.Play(EyesClosed);
        }
    }

    // Optional: You can add transitions callbacks or events here if needed
}
