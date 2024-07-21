using UnityEngine;

public class GrassDetailGizmo : MonoBehaviour
{
    public float detectionRadius = 3f;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}