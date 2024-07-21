using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterBuilding : MonoBehaviour
{
    public List<Transform> LevelTeleportTransforms;

    private bool canEnterBuilding = false;
    private GameObject cachedPlayer;

    public bool IsInBuilding = false;

    public int floorIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize as needed
    }

    // Update is called once per frame
    void Update()
    {
        if (IsInBuilding)
        {
            // use the arrow keys to travel the different floors
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                // go up the floor

                floorIndex++;
                PlayerFloorTeleportation();

            }

            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                // go down the floor (if not highest floor, ignore, there is an exit door the player could use instead already)
                floorIndex--;
                PlayerFloorTeleportation();
            }
        }


        // Check for player input to teleport
        if (canEnterBuilding && Input.GetKeyUp(KeyCode.F))
        {
            floorIndex = 0;
            IsInBuilding = true;
            PlayerFloorTeleportation();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cachedPlayer = other.gameObject;
            canEnterBuilding = true;
            GameManager.Instance.PlayerInteractionPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canEnterBuilding = false;
            IsInBuilding = false;
            GameManager.Instance.PlayerInteractionPrompt.SetActive(false);
        }
    }

    private void PlayerFloorTeleportation()
    {
        if (LevelTeleportTransforms.Count > 0 && cachedPlayer != null)
        {

            GameManager.Instance.BlinkingController.PlayBlinkingOpen();

            cachedPlayer.GetComponent<CharacterController>().enabled = false;

            cachedPlayer.transform.position = LevelTeleportTransforms[floorIndex].position;
            

            cachedPlayer.GetComponent<CharacterController>().enabled = true;

            GameManager.Instance.PlayerInteractionPrompt.SetActive(false);

            canEnterBuilding = false;
        }
    }
}
