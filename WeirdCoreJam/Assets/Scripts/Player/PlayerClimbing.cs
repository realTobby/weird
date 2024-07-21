using UnityEngine;

public class PlayerClimbing : MonoBehaviour
{
    public float climbSpeed = 8f;
    public float wallJumpForce = 10f; // Adjust this value to change the jump force
    private bool isClimbing = false;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (isClimbing)
        {
            HandleClimbing();
            HandleWallJump();
        }
    }

    void HandleClimbing()
    {
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(0, vertical, 0);
        controller.Move(move * climbSpeed * Time.deltaTime);
    }

    void HandleWallJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            isClimbing = false;
            GetComponent<PlayerGravity>().enabled = true; // Enable gravity

            // Apply a force away from the wall
            Vector3 wallJumpDirection = -transform.forward + Vector3.up; // Modify this to adjust the direction
            controller.Move(wallJumpDirection * wallJumpForce * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Climbable"))
        {
            isClimbing = true;
            GetComponent<PlayerGravity>().enabled = false; // Disable gravity
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Climbable"))
        {
            isClimbing = false;
            GetComponent<PlayerGravity>().enabled = true; // Enable gravity
        }
    }

    public bool IsClimbing()
    {
        return isClimbing;
    }
}
