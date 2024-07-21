using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetchItem : MonoBehaviour
{
    private GameObject cachedPlayer;

    public bool CanPickup = false;

    public AbstractTask taskRef;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CanPickup && Input.GetKeyUp(KeyCode.F))
        {
            CanPickup = false;
            OnPickup();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            cachedPlayer = other.gameObject;
            GameManager.Instance.PlayerInteractionPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            CanPickup = false;

            GameManager.Instance.PlayerInteractionPrompt.SetActive(false);
        }
    }

    private void OnPickup()
    {
        GameManager.Instance.PlayerInteractionPrompt.SetActive(false);

        GameManager.Instance.TaskManager.CompleteTask(taskRef);

        Destroy(this.gameObject);
    }
}
