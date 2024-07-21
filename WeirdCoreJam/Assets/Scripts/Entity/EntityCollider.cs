using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EntityCollider : MonoBehaviour
{

    public string TAG;

    public AudioSource hitSFX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public UnityEvent OnHitDetected;

    private void OnCollisionEnter(Collision other)
    {
        if(other.transform.CompareTag( TAG ))
        {
            hitSFX.Play();
            Destroy(other.gameObject);
            OnHitDetected?.Invoke();
            
        
        }
    }

}
