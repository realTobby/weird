using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public EnemyEntity myNPC;


    public DialogueData dialogueData;  // Reference to the scriptable object
    public Vector3 offset;

    private bool isTalking;
    private GameObject cachedPlayer;

    void Start()
    {
        isTalking = false;
        myNPC = GetComponent<EnemyEntity>();

    }

    void Update()
    {
        if (CanTalk && Input.GetKeyUp(KeyCode.F))
        {
            CanTalk = false;
            OnTalk();
        }
    }

    public bool CanTalk { get; set; } = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isTalking) return;
        if (other.transform.CompareTag("Player"))
        {
            cachedPlayer = other.gameObject;
            CanTalk = true;
            GameManager.Instance.PlayerInteractionPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            CanTalk = false;
            GameManager.Instance.DialougeSystem.EndDialogue();
            GameManager.Instance.PlayerInteractionPrompt.SetActive(false);
        }
    }

    private void OnTalk()
    {
        var guardBlock = GetComponent<GuardBlock>();
        if (guardBlock != null)
        {
            guardBlock.OnTalk();
        }

        GameManager.Instance.PlayerInteractionPrompt.SetActive(false);
        if (dialogueData != null)
        {
            int dialougeID = Convert.ToInt32(dialogueData.dialogueID);

            var dialogueFull = DialogueLoader.GetDialogueForNPCWithDialogueIDFromResources(myNPC.dataHolder.EntityName, dialougeID);
            var startNode = DialogueLoader.GetNodeById(1); // Assuming 1 is the starting node ID

            DialogueNode startingLine = startNode;
            if (startingLine != null)
            {
                GameManager.Instance.DialougeSystem.StartDialogue(startingLine, this.transform, offset);
            }
            else
            {
                Debug.LogError("Dialogue start line is null for NPC: " + gameObject.name);
            }
        }
        else
        {
            Debug.LogError("DialogueData is not assigned for NPC: " + gameObject.name);
        }
    }
}
