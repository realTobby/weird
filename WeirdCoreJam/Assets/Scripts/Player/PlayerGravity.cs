using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGravity : MonoBehaviour
{
    public float gravity = -9.81f;
    private CharacterController controller;
    private PlayerState playerState;
    
    public Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerState = GetComponent<PlayerState>();
        
    }

    void Update()
    {
        

        if (playerState.isGrounded && velocity.y < 0)
        {
            velocity.y = -1.25f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void Jump(float jumpHeight)
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        playerState.isGrounded = false;
    }
}
