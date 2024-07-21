using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentScene : MonoBehaviour
{


    public static PersistentScene _instance = null;
    public static PersistentScene Instance => _instance;




    // Start is called before the first frame update
    void Start()
    {
        if(PersistentScene.Instance == null)
        {
            _instance = this;
        }else{
            Destroy(this.gameObject);
        }


        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
