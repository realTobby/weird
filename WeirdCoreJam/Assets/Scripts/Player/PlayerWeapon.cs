using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public GameObject Prefab_PlayerProjectile;
    public Transform firePoint; // Point from where the projectile will be fired
    public float projectileSpeed = 20f; // Speed of the projectile

    void Update()
    {
        //When left click, instantiate a projectile and shoot it towards the direction the player is looking
        //if (Input.GetButtonDown("Fire1"))
        //{
        //    Shoot();
        //}
    }

    void Shoot()
    {
        // Instantiate the projectile at the fire point
        GameObject projectile = Instantiate(Prefab_PlayerProjectile, firePoint.position, firePoint.rotation);

        // Get the Rigidbody component of the projectile
        Rigidbody rb = projectile.GetComponent<Rigidbody>();


        // also 

        // If the projectile has a Rigidbody, apply force to it
        if (rb != null)
        {
            rb.velocity = Camera.main.transform.forward * projectileSpeed;
        }

        projectile.GetComponentInChildren<QuadRotateToPlayer>().player = this.transform;


        Destroy(projectile, 1f);

    }
}
