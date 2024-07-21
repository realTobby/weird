using UnityEngine;

public class GrassInteraction : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float detectionRadius = 3f;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) < detectionRadius)
        {
            MoveAwayFromPlayer();
        }
    }

    void MoveAwayFromPlayer()
    {
        Vector3 directionAwayFromPlayer = (transform.position - player.position).normalized;
        transform.position += directionAwayFromPlayer * moveSpeed * Time.deltaTime;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
