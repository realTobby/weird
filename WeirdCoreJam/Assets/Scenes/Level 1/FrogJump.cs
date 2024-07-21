using System.Collections;
using UnityEngine;

public class FrogJump : MonoBehaviour
{
    public float jumpForce = 5f;
    public float jumpInterval = 2f;
    public float waitTimeBeforeNextJump = 1f;
    public LayerMask groundLayer;
    private Rigidbody rb;
    private bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(JumpRoutine());
    }

    IEnumerator JumpRoutine()
    {
        while (true)
        {
            if (IsGrounded())
            {
                Debug.Log("Frog is grounded, preparing to jump.");
                Jump();
                yield return new WaitForSeconds(waitTimeBeforeNextJump);
            }
            else
            {
                Debug.Log("Frog is not grounded.");
            }
            yield return new WaitForSeconds(jumpInterval);
        }
    }

    void Jump()
    {
        Vector3 jumpDirection = transform.forward + Vector3.up;
        rb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
        Debug.Log("Frog jumped with force: " + jumpDirection * jumpForce);
    }

    bool IsGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f, groundLayer))
        {
            Debug.Log("Raycast hit: " + hit.collider.name);
            return true;
        }
        return false;
    }
}
