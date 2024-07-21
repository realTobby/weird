using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI; // Import the UI namespace

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    public bool IsGamePaused = false;

    void Awake()
    {
        _instance = this;

        
    }

    public GameObject UI_PauseMenu;
    public Slider mouseSensitivitySlider; // Add a reference to the mouse sensitivity slider
    public Slider pixelationSlider; // Add a reference to the pixelation slider

    public CameraController cameraController; // Reference to the CameraController script
    public PixelationEffect pixelationEffect; // Reference to the PixelationEffect script

    public GameObject PrefabSpeechBubble;

    public GameObject PlayerInteractionPrompt;

    public TaskManager TaskManager;
    public GameObject Player;

    public DialogueSystem DialougeSystem;

    public BlinkingController BlinkingController;

    private CharacterController characterController;


    

    void Start()
    {
        // Set initial slider values
        mouseSensitivitySlider.value = cameraController.mouseSensitivity;
        pixelationSlider.value = pixelationEffect.pixelationAmount;

        // Add listeners to the sliders
        mouseSensitivitySlider.onValueChanged.AddListener(SetMouseSensitivity);
        pixelationSlider.onValueChanged.AddListener(SetPixelationAmount);

        ClosePauseMenu();

        Player = GameObject.FindGameObjectWithTag("Player");

        characterController = Player.GetComponent<CharacterController>();
    }

    public void OpenPauseMenu()
    {
        IsGamePaused = true;
        Cursor.lockState = CursorLockMode.None;
        UI_PauseMenu.SetActive(true);
    }

    public void ClosePauseMenu()
    {
        Cursor.lockState = CursorLockMode.Locked;
        IsGamePaused = false;
        UI_PauseMenu.SetActive(value: false);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OpenPauseMenu();
        }
    }

    // Methods to set the values
    public void SetMouseSensitivity(float value)
    {
        cameraController.mouseSensitivity = value;
    }

    public void SetPixelationAmount(float value)
    {
        pixelationEffect.pixelationAmount = (int)value;
    }

    public GameObject World_Hub;
    public GameObject World_Test;


    public void GoToHub(Transform teleport)
    {
        ClosePauseMenu();
        IsGamePaused = false;

        World_Test.SetActive(false);
        World_Hub.SetActive(true);




        Player.GetComponent<CharacterController>().enabled = false;
        if(teleport != null)
            Player.transform.position = teleport.position;

        Player.GetComponent<CharacterController>().enabled = true;
    }

    public void GoToTest(Transform teleport)
    {
        World_Test.SetActive(true);
        World_Hub.SetActive(false);

        if(teleport != null)
            GameObject.FindGameObjectWithTag("Player").transform.position = teleport.position;

    }

    public AudioSource BGMAudioSource;

    public void PlayMusic(AudioClip audioClip)
    {

        if (BGMAudioSource.clip == audioClip) return;


        BGMAudioSource.Stop();
        BGMAudioSource.clip = audioClip;
        BGMAudioSource.Play();
    }

    public void TeleportPlayer(Vector3 newPosition)
    {
        characterController.enabled = false;
        Player.transform.position = newPosition;
        characterController.enabled = true;
    }

}
