using UnityEngine;

public class GuardBlock : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float blockDistance = 2.0f; // Distance in front of the player to block
    public float proximityThreshold = 5.0f; // Distance within which the guard adjusts the position
    private Transform quad; // Reference to the Quad child
    public Collider solidCollider; // Reference to the solid collider

    public Transform playerTeleportPoint;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Find the Quad child object
        quad = transform.Find("Quad");
        if (quad == null)
        {
            Debug.LogError("Quad child not found!");
        }

    }

    void Update()
    {
        BlockPlayer();
    }

    bool IsPlayerInProximity()
    {
        // Check if the player is within the proximity threshold
        return Vector3.Distance(player.position, transform.position) <= proximityThreshold;
    }

    void BlockPlayer()
    {
        // Calculate the direction to the player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // Calculate the block position based on the intersection with the collider edge
        Vector3 blockPosition = GetColliderEdgePosition(player.position, directionToPlayer);

        // Adjust the block position
        blockPosition.y = quad.position.y; // Keep the quad on the same Y level
        quad.position = blockPosition;

        // Make the quad face the player
        //quad.LookAt(player);

        // Debug: Draw a line to visualize the block position
        Debug.DrawLine(quad.position, player.position, Color.red, 0.1f);
    }

    Vector3 GetColliderEdgePosition(Vector3 playerPosition, Vector3 directionToPlayer)
    {
        // Raycast to find the intersection point with the collider edge
        Ray ray = new Ray(playerPosition, -directionToPlayer);
        RaycastHit hit;

        if (solidCollider.Raycast(ray, out hit, Mathf.Infinity))
        {
            // Return the hit point as the block position
            return hit.point;
        }
        else
        {
            // Default block position if no intersection found
            return playerPosition - directionToPlayer * blockDistance;
        }
    }

    public GameObject SpawnMap;
    public GameObject LongAreaObject;


    public void OnTalk()
    {

        //DialogueSystem.Instance.StartDialogue(dialogueLines, transform, new Vector3(0, 2, 0));

        SpawnMap.SetActive(false);
        LongAreaObject.SetActive(true);

        if(playerTeleportPoint == null)
        {
            GameManager.Instance.TeleportPlayer(new Vector3(0, 10, 0));
        }
        else
        {
            GameManager.Instance.TeleportPlayer(playerTeleportPoint.position);
        }

        

    }
}
