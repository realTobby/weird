using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class PlayerState : MonoBehaviour
    {
        public Transform groundCheck;
        public float groundDistance = 0.4f;
        public LayerMask groundMask;

        public bool isGrounded;
        public bool isClimbing;
        public bool isMoving;
        public float lastTimeGrounded;
public PlayerMovement plaayerMov;

        public Surface.SurfaceType currentSurfaceType;

    void Start()
    {
        plaayerMov = GetComponent<PlayerMovement>();
    }

        public Transform lastGroundCheck;

        void Update()
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded)
            {
                lastTimeGrounded = Time.time;

                // Perform a raycast to detect the surface type
                RaycastHit hit;
                if (Physics.Raycast(groundCheck.position, Vector3.down, out hit, groundDistance + 1f, groundMask))
                {
                    Surface.Surface surface = hit.collider.GetComponent<Surface.Surface>();

                    
                    if (surface != null)
                    {
                        if(surface.transform == lastGroundCheck) return;

                        lastGroundCheck = surface.transform;
                        currentSurfaceType = surface.surfaceType;

                        plaayerMov.PlayFootstep();
                    }
                }
                
            }
        }
    }
