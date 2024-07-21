using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTrigger : MonoBehaviour
{
    public List<GameObject> OnTriggerEnterMapsToActivate;
    public List<GameObject> OnTriggerEnterMapsToDeactivate;
    public List<GameObject> OnTriggerExitMapsToActivate;
    public List<GameObject> OnTriggerExitMapsToDeactivate;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is the player or any other relevant object
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.BlinkingController.PlayBlinkingOpen();


            // Deactivate maps that need to be deactivated on trigger enter
            foreach (GameObject mapRoot in OnTriggerEnterMapsToDeactivate)
            {
                mapRoot.SetActive(false);
            }

            // Activate maps that need to be activated on trigger enter
            foreach (GameObject mapRoot in OnTriggerEnterMapsToActivate)
            {
                mapRoot.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object that exited the trigger is the player or any other relevant object
        if (other.CompareTag("Player"))
        {
            // Deactivate maps that need to be deactivated on trigger exit
            foreach (GameObject mapRoot in OnTriggerExitMapsToDeactivate)
            {
                mapRoot.SetActive(false);
            }

            // Activate maps that need to be activated on trigger exit
            foreach (GameObject mapRoot in OnTriggerExitMapsToActivate)
            {
                mapRoot.SetActive(true);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
