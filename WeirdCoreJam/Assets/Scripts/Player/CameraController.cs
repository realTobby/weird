using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 50f;
    public Transform playerBody;
    public float rayLength = 100f; // Increased the ray length for better reach

    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (GameManager.Instance.IsGamePaused) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

        HandleRaycasting();
    }

    void HandleRaycasting()
    {
        //if (DialogueSystem.Instance == null) return;

        //GameObject indicator = DialogueSystem.Instance.DialogueOptionIndicator;
        //if (indicator == null) return;

        //// Create a pointer event
        //PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
        //{
        //    pointerId = -1,
        //};

        //// Set pointer position
        //pointerEventData.position = new Vector2(Screen.width / 2, Screen.height / 2);

        //// Raycast using the EventSystem
        //List<RaycastResult> results = new List<RaycastResult>();
        //EventSystem.current.RaycastAll(pointerEventData, results);

        //bool hitDialogueOption = false;

        //foreach (RaycastResult result in results)
        //{
        //    if (result.gameObject.CompareTag("DialogueOption"))
        //    {
        //        hitDialogueOption = true;
        //        indicator.transform.position = result.gameObject.transform.position;
        //        OptionComponent optionComponent = result.gameObject.GetComponent<OptionComponent>();
        //        if (optionComponent != null && Input.GetMouseButtonDown(0))
        //        {
        //            DialogueSystem.Instance.SelectOption(optionComponent.responseNode);
        //            break;
        //        }
        //    }
        //}

        //indicator.SetActive(hitDialogueOption);
    }



    // This method is called to draw Gizmos in the Scene view.
    void OnDrawGizmos()
    {
        if (Camera.main != null)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            Gizmos.color = Color.red;
            Gizmos.DrawRay(ray.origin, ray.direction * rayLength);
        }
    }
}
