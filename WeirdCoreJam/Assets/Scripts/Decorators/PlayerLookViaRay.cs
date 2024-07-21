using System;
using UnityEngine;

public class PlayerLookViaRay : MonoBehaviour
{
    public float lookDistance = 10f; // Maximum distance to check for looking
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfLookedAt();
    }

    public SceneDecorator parentDecorator;

    void CheckIfLookedAt()
    {
        // Check if the object is within the camera's view frustum
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        Collider objectCollider = GetComponent<Collider>();

        if (GeometryUtility.TestPlanesAABB(planes, objectCollider.bounds))
        {
            // Optionally, you can still check the distance if you want to limit how far the player can look at the object
            if (Vector3.Distance(mainCamera.transform.position, transform.position) <= lookDistance)
            {
                OnLookedAt();
            }
            else
            {
                isLookingat = false;
                updateLookStatus();
            }
        }
        else
        {
            isLookingat = false;
            updateLookStatus();
        }
    }

    bool isLookingat = false;

    void OnLookedAt()
    {
        // Trigger the action here, for example:
        //Debug.Log("Player is looking at " + gameObject.name);
        // You can add more actions or triggers here
        isLookingat = true;

        // Tell all objects from that trigger to invert the animator apply root motion variable
        updateLookStatus();
    }

    private void updateLookStatus()
    {
        for (int i = 0; i < parentDecorator.transform.childCount; i++)
        {
            if (parentDecorator.transform.GetChild(i).CompareTag("TRIGGER")) continue;
            parentDecorator.transform.GetChild(i).GetComponent<Animator>().applyRootMotion = isLookingat;
        }
    }
}
