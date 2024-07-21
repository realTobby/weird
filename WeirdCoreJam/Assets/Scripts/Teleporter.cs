using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    public enum LevelType
    {
        Hub,
        Level1,
        Level2,
        Level3,
        DEBUG,
        Level4
    }

    public LevelType DestinationType;

    public Transform destinationTransform;

    public bool WasTeleportedWith = false;

    public bool IsCake = false;

    public void LoadSceneTranslateDestinationTypeToScene(GameObject Player)
    {
        var characterController = Player.GetComponent<CharacterController>();
        characterController.enabled = false;
        characterController.transform.position = new Vector3(0,0,0);;
        characterController.enabled = true;

        DontDestroyOnLoad(this.transform.parent.gameObject);

        WasTeleportedWith = true;

        switch(DestinationType)
        {
            case LevelType.Hub:
                SceneManager.LoadScene(0);
            break;
            case LevelType.Level1:
                SceneManager.LoadScene(1);
            break;
            case LevelType.Level2:
                SceneManager.LoadScene(2);
            break;
            case LevelType.DEBUG:
                //SceneManager.LoadScene(0);
                GameManager.Instance.GoToTest(null);
            break;
        }

        

        if(destinationTransform != null)
            Player.transform.position = destinationTransform.position;
        


    }

    // Start is called before the first frame update
    void Start()
    {
        if(WasTeleportedWith)
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Player"))
        {
            print("player teleport");
            other.transform.parent.position = new Vector3(0,2,0);
            //GameManager.Instance.GoToTest(destinationTransform);
            LoadSceneTranslateDestinationTypeToScene(other.gameObject);            

        }
    }

}
