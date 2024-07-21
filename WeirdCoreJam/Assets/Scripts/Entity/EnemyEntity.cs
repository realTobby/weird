using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EnemyEntity : MonoBehaviour
{

    public EntityDataSO dataHolder;

    private QuadRotateToPlayer rotateToPlayer;

    public EntityCollider collisonDetector;

    public int MaxHP = 1;
    public int HP = 0;



    // Start is called before the first frame update
    void Start()
    {

        if(dataHolder != null)
        {
            if (dataHolder.EntityMaterial !=  null)
            {
            MaxHP = dataHolder.MAXHP;
            HP = MaxHP;

            GetComponentInChildren<Renderer>().material = dataHolder.EntityMaterial;

            
            }
            
        }
        

        if(collisonDetector != null)
            collisonDetector.OnHitDetected.AddListener(OnEnemyHit);

        rotateToPlayer = GetComponentInChildren<QuadRotateToPlayer>();
        rotateToPlayer.player = GameObject.FindGameObjectWithTag("Player").transform;

        // get values from data object and set them to this entity
        



    }

    private void OnEnemyHit()
    {
        HP--;
        if(HP<=0)
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
