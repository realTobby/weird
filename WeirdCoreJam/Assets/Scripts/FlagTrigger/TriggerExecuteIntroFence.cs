using System.Collections;
using UnityEngine;

public class TriggerExecuteIntroFence : MonoBehaviour
{
    public string FlagTriggerName = "FlagTrigger-IntroFence";
    public float rotationSpeed = 10f;
    public float targetRotationX = -10f;
    private bool shouldRotate = false;

    private void Start()
    {
        // Register this script to listen for flag updates
        GameManager.Instance.TaskManager.OnFlagUpdated.AddListener(OnFlagSet);
    }

    private void OnDestroy()
    {
        // Unregister this script when it's destroyed
        GameManager.Instance.TaskManager.OnFlagUpdated.RemoveListener(OnFlagSet);
    }

    private void OnFlagSet(string flag)
    {
        if (flag == FlagTriggerName)
        {
            OnExecute();
        }
    }

    public void OnExecute()
    {
        shouldRotate = true;
    }

    private void Update()
    {
        if (shouldRotate)
        {
            float step = rotationSpeed * Time.deltaTime;
            Vector3 newRotation = transform.eulerAngles;
            newRotation.x = Mathf.MoveTowardsAngle(transform.eulerAngles.x, -targetRotationX, step);

            transform.eulerAngles = newRotation;

            if (Mathf.Approximately(newRotation.x, targetRotationX))
            {
                shouldRotate = false; // Stop rotating when target is reached
                Destroy(this); // hopefully this only destroy so MonoBehaviour not the gameobject lol
            }
        }
    }
}
