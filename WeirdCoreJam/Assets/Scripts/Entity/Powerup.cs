using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public AudioSource sfx;
    public bool collected = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Player") && collected == false)
        {
            Destroy(this.gameObject.GetComponent<Renderer>());
            sfx.Play();
            collected = true;
            Destroy(this.gameObject.transform.parent.gameObject, 5f);

            other.GetComponent<PlayerMovement>().addedSpeed += 1.25f;
            other.GetComponent<PlayerMovement>().addedSpeedDecayTimer = 0;
        }
    }

}
