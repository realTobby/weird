using System.Collections.Generic;
using UnityEngine;
using Surface;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerGravity))]
[RequireComponent(typeof(PlayerClimbing))]
[RequireComponent(typeof(PlayerState))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 2f;
    public float sprintSpeed = 4f;
    public float jumpHeight = 3f;

    private CharacterController controller;
    private PlayerGravity playerGravity;
    private PlayerClimbing playerClimbing;
    private PlayerState playerState;

    public Transform playerTransform; // Reference to the player transform
    public float currentSpeed;
    public AudioSource jumpSFX;
    public AudioSource footstepSFX;

    public AudioClip grassFootstep;
    public AudioClip waterFootstep;
    public AudioClip concreteFootstep;

    private Dictionary<SurfaceType, AudioClip> footstepSounds;
    private SurfaceType currentSurface;

    public float addedSpeed = 0f;
    public float jumpBoost = 1f;
    public float jumpBoostTimeWindow = 1f;
    public float addedSpeedDecayTime = 2f;
    public float addedSpeedDecayTimer = 0f;

    private float footstepTimer = 0f;
    private float footstepInterval;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerGravity = GetComponent<PlayerGravity>();
        playerClimbing = GetComponent<PlayerClimbing>();
        playerState = GetComponent<PlayerState>();
        currentSpeed = speed;

        footstepSounds = new Dictionary<SurfaceType, AudioClip>
        {
            { SurfaceType.Grass, grassFootstep },
            { SurfaceType.Water, waterFootstep },
            { SurfaceType.Concrete, concreteFootstep }
        };
    }

    void Update()
    {
        if (GameManager.Instance.IsGamePaused) return;

        HandleMovement();
        HandleJumping();
    }

    void HandleMovement()
    {
        playerState.isMoving = false;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : speed;

        // Apply added speed if any
        if (addedSpeed > 0)
        {
            currentSpeed += addedSpeed;

            // Decay the added speed over time
            addedSpeedDecayTimer += Time.deltaTime;
            if (addedSpeedDecayTimer >= addedSpeedDecayTime)
            {
                addedSpeed = 0;
                addedSpeedDecayTimer = 0f;
            }
        }

        // Calculate movement direction relative to the player's orientation
        Vector3 forward = playerTransform.forward;
        Vector3 right = playerTransform.right;

        // Keep the movement direction parallel to the ground
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * z + right * x;

        if (!playerClimbing.IsClimbing())
        {
            controller.Move(move * currentSpeed * Time.deltaTime);
            playerState.isMoving = move.magnitude > 0;

            if (move.magnitude > 0 && controller.isGrounded)
            {
                footstepTimer += Time.deltaTime;
                footstepInterval = 1.5f / currentSpeed; // Adjust interval based on current speed

                if (footstepTimer >= footstepInterval)
                {
                    PlayFootstep();
                    footstepTimer = 0f;
                }
            }
        }
    }


    void HandleJumping()
    {
        if (Input.GetButtonDown("Jump") && playerState.isGrounded && !playerClimbing.IsClimbing())
        {
            jumpSFX.Play();
            playerGravity.Jump(jumpHeight);

            float currentTime = Time.time;
            if (currentTime - playerState.lastTimeGrounded <= jumpBoostTimeWindow)
            {
                addedSpeed += jumpBoost;
                addedSpeedDecayTimer = 0f; // Reset decay timer when jump boost is added
            }
            else
            {
                addedSpeed = 0f;
            }
        }
    }


    public void PlayFootstep()
    {
        if (footstepSounds.TryGetValue(playerState.currentSurfaceType, out AudioClip clip))
        {
            footstepSFX.clip = clip;
            footstepSFX.Play();
        }
    }
}
