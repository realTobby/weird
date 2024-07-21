using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance { get; private set; }

    [Header("UI Elements")]
    public GameObject PrefabSpeechBubble;
    public Vector3 DefaultTextRightOffset;
    public UIDialogueDashboard UIDialogueFunctions;

    [Header("Settings")]
    public float typingSpeed = 0.05f;
    public float fastTypingSpeed = 0.01f;

    private Coroutine dialogueCoroutine;
    private Transform cacheLastNPCTransform;
    private DialogueNode currentNode;
    
    
    private GameObject speechBubbleGO;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);

        UIDialogueFunctions = this.gameObject.AddComponent<UIDialogueDashboard>();
    }

    private void Update()
    {
        UIDialogueFunctions.HandleRaycasting();
    }

    public void StartDialogue(DialogueNode startingNode, Transform npcTransform, Vector3 offset)
    {
        currentNode = startingNode;
        GameManager.Instance.PlayerInteractionPrompt.SetActive(false);

        if (dialogueCoroutine != null)
        {
            StopCoroutine(dialogueCoroutine);
        }

        InitializeDialogue(npcTransform, offset);
        dialogueCoroutine = StartCoroutine(DisplayDialogue());
    }

    private void InitializeDialogue(Transform npcTransform, Vector3 offset)
    {
        cacheLastNPCTransform = npcTransform;
        speechBubbleGO = Instantiate(PrefabSpeechBubble, npcTransform);
        speechBubbleGO.transform.localPosition += offset;

        UIDialogueFunctions.optionsRoot = speechBubbleGO.transform.Find("Canvas/textBox/OptionsRoot");
    }

    private IEnumerator DisplayDialogue()
    {
        while (currentNode != null)
        {
            Debug.Log("Executing node: " + currentNode.text);
            yield return ExecuteNode(currentNode);

            if (currentNode == null)
            {
                Debug.Log("Reached end of dialogue tree.");
                EndDialogue();
                yield break;
            }
        }

        EndDialogue();
    }



    private IEnumerator ExecuteNode(DialogueNode node)
    {
        if (node == null)
        {
            Debug.LogError("ExecuteNode called with null node.");
            yield break;
        }

        

        if (!string.IsNullOrEmpty(node.addTask))
        {
            GameManager.Instance.TaskManager.AddTaskByName(node.addTask);
        }

        if (!string.IsNullOrEmpty(node.completeTask))
        {
            GameManager.Instance.TaskManager.CompleteTaskByName(node.completeTask);
        }

        if (!string.IsNullOrEmpty(node.checkTaskName))
        {
            bool isTaskCompleted = GameManager.Instance.TaskManager.CheckTaskCompletion(node.checkTaskName);
            currentNode = isTaskCompleted ? DialogueLoader.GetNodeById(node.checkTaskOptions[0].responseID) : DialogueLoader.GetNodeById(node.checkTaskOptions[1].responseID);
            yield break;
        }

        yield return ShowText(node.text);

        if (node.options != null && node.options.Count > 0)
        {
            yield return HandleOptions(node.options);
        }
        else if (node.nextID != 0)
        {
            currentNode = DialogueLoader.GetNodeById(node.nextID);
            Debug.Log("Moving to next node via nextID: " + (currentNode != null ? currentNode.text : "null"));
        }
        else
        {
            currentNode = node.nextLine;
            Debug.Log("Current node has no options and no nextID, moving to next node: " + (currentNode != null ? currentNode.text : "null"));
        }
    }



    private IEnumerator HandleOptions(List<DialogueOption> options)
    {
        UIDialogueFunctions.SetDialogueOptions(options);
        UIDialogueFunctions.optionsRoot.gameObject.SetActive(true);
        yield return WaitForOptionSelection();
        UIDialogueFunctions.optionsRoot.gameObject.SetActive(false);

        if (UIDialogueFunctions.selectedOption >= 0 && UIDialogueFunctions.selectedOption < options.Count)
        {
            currentNode = options[UIDialogueFunctions.selectedOption].response;
            Debug.Log("Selected option leads to: " + currentNode.text);
        }
    }

    private IEnumerator WaitForOptionSelection()
    {
        UIDialogueFunctions.selectedOption = -1;
        while (UIDialogueFunctions.selectedOption == -1)
        {
            yield return null;
        }
    }

    private IEnumerator ShowText(string text)
    {
        TextMeshProUGUI dialogueText = speechBubbleGO.GetComponentInChildren<TextMeshProUGUI>();
        dialogueText.text = "";
        foreach (char letter in text)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
    }

    public void EndDialogue()
    {
        if (speechBubbleGO != null)
        {
            Destroy(speechBubbleGO);
        }

        if (dialogueCoroutine != null)
        {
            StopCoroutine(dialogueCoroutine);
        }

        GameManager.Instance.PlayerInteractionPrompt.SetActive(false);
    }

    private DialogueNode GetNextNode(DialogueNode currentNode)
    {
        return DialogueLoader.GetNextNode(currentNode);
    }
}

public class UIDialogueDashboard : MonoBehaviour
{
    public GameObject DialogueOptionIndicator;
    public Transform optionsRoot;

    public int selectedOption;

    public void SetDialogueOptions(List<DialogueOption> options)
    {
        DialogueOptionIndicator = optionsRoot.transform.GetChild(0).gameObject;

        if (options == null || options.Count == 0) return;

        var option1 = optionsRoot.GetChild(1);
        var option2 = optionsRoot.GetChild(2);

        option1.gameObject.SetActive(options.Count > 0);
        option2.gameObject.SetActive(options.Count > 1);

        var option1Text = option1.GetComponentInChildren<TextMeshProUGUI>();
        var option2Text = option2.GetComponentInChildren<TextMeshProUGUI>();

        if (options.Count > 0) option1Text.text = options[0].text;
        if (options.Count > 1) option2Text.text = options[1].text;

        var option1Component = option1.GetComponent<OptionComponent>();
        if (option1Component == null) option1Component = option1.gameObject.AddComponent<OptionComponent>();
        option1Component.responseLine = DialogueLoader.GetNodeById(options[0].responseID);

        var option2Component = option2.GetComponent<OptionComponent>();
        if (option2Component == null) option2Component = option2.gameObject.AddComponent<OptionComponent>();
        option2Component.responseLine = DialogueLoader.GetNodeById(options[1].responseID);

        DialogueOptionIndicator.SetActive(true);

    }

    public void HandleRaycasting()
    {
        if (DialogueSystem.Instance == null) return;

        if (DialogueOptionIndicator == null) return;

        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
        {
            pointerId = -1,
            position = new Vector2(Screen.width / 2, Screen.height / 2)
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        bool hitDialogueOption = false;

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("DialogueOption"))
            {
                hitDialogueOption = true;
                DialogueOptionIndicator.transform.position = result.gameObject.transform.position;
                OptionComponent optionComponent = result.gameObject.GetComponent<OptionComponent>();
                if (optionComponent != null && Input.GetMouseButtonDown(0))
                {
                    selectedOption = result.gameObject.transform.GetSiblingIndex() - 1;
                    break;
                }
            }
        }

        DialogueOptionIndicator.SetActive(hitDialogueOption);
    }

}
public class OptionComponent : MonoBehaviour
{
    public DialogueNode responseLine;
}

public static class DialogueLoader
{
    private static Dictionary<int, DialogueNode> dialogueNodes;

    public static DialogueContainer GetDialogueForNPCWithDialogueIDFromResources(string npcName, int id)
    {
        string path = $"NPCData/{npcName}/{id}";
        TextAsset jsonFile = Resources.Load<TextAsset>(path);
        if (jsonFile == null)
        {
            Debug.LogError($"Dialogue file not found at path: {path}");
            return null;
        }

        var dialogueContainer = JsonUtility.FromJson<DialogueContainer>(jsonFile.text);
        if (dialogueContainer == null)
        {
            Debug.LogError("DialogueContainer is null after deserialization.");
            return null;
        }

        dialogueNodes = new Dictionary<int, DialogueNode>();
        foreach (var node in dialogueContainer.dialogueNodes)
        {
            dialogueNodes[node.id] = node;
        }


        LinkNodes(dialogueNodes);

        PrintDialogueTree();

        return dialogueContainer;
    }

    private static void LinkNodes(Dictionary<int, DialogueNode> nodes)
    {
        foreach (var node in nodes.Values)
        {
            if (node.options != null)
            {
                foreach (var option in node.options)
                {
                    if (nodes.TryGetValue(option.responseID, out DialogueNode responseNode))
                    {
                        option.response = responseNode;
                    }
                }
            }

            if (node.checkTaskOptions != null)
            {
                foreach (var taskOption in node.checkTaskOptions)
                {
                    if (nodes.TryGetValue(taskOption.responseID, out DialogueNode responseNode))
                    {
                        taskOption.response = responseNode;
                    }
                }
            }

            if (node.nextID != -1 && nodes.TryGetValue(node.nextID, out DialogueNode nextNode))
            {
                node.nextLine = nextNode;
            }
        }
    }


    public static void PrintDialogueTree()
    {
        foreach (var node in dialogueNodes.Values)
        {
            Debug.Log($"Node {node.id}: {node.text}");
            if (node.options != null)
            {
                foreach (var option in node.options)
                {
                    Debug.Log($"  Option '{option.text}' leads to node {option.responseID}");
                }
            }
            if (node.nextLine != null)
            {
                Debug.Log($"  Next node is {node.nextLine.id}: {node.nextLine.text}");
            }
        }
    }

    public static DialogueNode GetNodeById(int id)
    {
        if (dialogueNodes != null && dialogueNodes.TryGetValue(id, out DialogueNode node))
        {
            return node;
        }
        return null;
    }

    public static DialogueNode GetNextNode(DialogueNode currentNode)
    {
        return currentNode?.nextLine;
    }
}

