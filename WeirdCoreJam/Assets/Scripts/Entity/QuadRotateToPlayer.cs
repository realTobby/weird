using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadRotateToPlayer : MonoBehaviour
{
    public Transform player; // Reference to the player's transform

    void Start()
    {
        try{
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }catch(Exception ex)
        {

        }

        



    }

    // Update is called once per frame
    void Update()
    {
        // Ensure the quad always looks at the player
        if (player != null)
        {

            // todo, also add tilt so its directly at the player

            Vector3 direction = transform.position - player.position;
            direction.y = 0; // Keep the rotation only in the horizontal plane
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
