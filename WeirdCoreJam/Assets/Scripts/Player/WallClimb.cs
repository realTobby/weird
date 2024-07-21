using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WallClimb : MonoBehaviour
{
    private PlayerClimbing controller;
    public PlayerMovement playerMovement;
    private UnityEngine.Vector3 climbStartPosition;
    private bool isClimbing;
    private float initialVelocity;


    void Start()
    {
        //playerMovement = GetComponent<PlayerMovement>();
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if(other.gameObject.layer == 7) return;

    //    if(other.gameObject.CompareTag("Player")) return;

    //    controller.OnCollisionWithClimbableObject();
    //}
}
