//using System.Collections;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using TMPro;
//using Unity.VisualScripting.Antlr3.Runtime.Tree;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;
//public class DialogueSystem : MonoBehaviour
//{
//    public static DialogueSystem Instance { get; private set; }

//    [Header("UI Elements")]
//    public GameObject PrefabSpeechBubble;
//    public Vector3 DefaultTextRightOffset;
//    public GameObject DialogueOptionIndicator;

//    [Header("Settings")]
//    public float typingSpeed = 0.05f;
//    public float fastTypingSpeed = 0.01f;

//    private GameObject speechBubbleGO;
//    private Coroutine dialogueCoroutine;
//    private GameObject cacheLastOptionsRoot, cacheLastOption1, cacheLastOption2;
//    private Transform cacheLastNPCTransform;
//    private Animator textBoxAnimator;
//    private bool isCurrentlyQuestion = false;
//    public List<DialogueLine> dialogueLines;
//    public int currentLine = 0;
//    public int selectedOption = -1;

//    private void Awake()
//    {
//        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
//        else Destroy(gameObject);
//    }

//    private void SetupDialogueOptions()
//    {
//        DialogueOptionIndicator = cacheLastOptionsRoot.transform.GetChild(0).gameObject;
//        cacheLastOption1.tag = "DialogueOption";
//        cacheLastOption2.tag = "DialogueOption";
//    }


//    public void StartDialogue(List<string> rawDialogueLines, Transform npcTransform, Vector3 offset)
//    {

//        string dialouge = string.Empty;


//        foreach (string line in rawDialogueLines)
//        {
//            dialouge += line + "\n";

//        }




//        if (speechBubbleGO != null) Destroy(speechBubbleGO);

//        dialogueLines = DialogueParser.ParseDialogue(rawDialogueLines);
//        currentLine = 0;
//        GameManager.Instance.PlayerInteractionPrompt.SetActive(false);

//        InitializeDialogue(npcTransform, offset);
//        SetupDialogueOptions();
//        dialogueCoroutine = StartCoroutine(DisplayDialogue());
//    }

//    private void InitializeDialogue(Transform npcTransform, Vector3 offset)
//    {
//        cacheLastNPCTransform = npcTransform;
//        speechBubbleGO = Instantiate(PrefabSpeechBubble, npcTransform);
//        speechBubbleGO.transform.localPosition += offset;
//        cacheLastOptionsRoot = speechBubbleGO.transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
//        cacheLastOption1 = cacheLastOptionsRoot.transform.GetChild(1).gameObject;
//        cacheLastOption2 = cacheLastOptionsRoot.transform.GetChild(2).gameObject;
//        textBoxAnimator = speechBubbleGO.GetComponentInChildren<Animator>();
//    }

//    private IEnumerator DisplayDialogue()
//    {
//        while (currentLine < dialogueLines.Count)
//        {
//            yield return ExecuteLine(dialogueLines[currentLine]);
//            currentLine++;
//        }
//        EndDialogue();
//    }

//    public IEnumerator ExecuteLine(DialogueLine line)
//    {
//        if (line.Command != null)
//        {
//            yield return line.Command.Execute(this);
//        }
//        else
//        {
//            yield return StartCoroutine(TypeText(line.Text));
//            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
//        }
//    }

//    public IEnumerator HandleQuestion(DialogueQuestionCommand command)
//    {
//        SetDialogueOptions(command.Option1, command.Option2);
//        cacheLastOptionsRoot.SetActive(true);
//        textBoxAnimator.Play("textBoxOpenOptions");
//        yield return StartCoroutine(TypeText(command.Question));
//        yield return StartCoroutine(WaitForOptionSelection());
//        cacheLastOptionsRoot.SetActive(false);
//        var selectedLines = selectedOption == 0 ? command.Option1Lines : command.Option2Lines;
//        foreach (var line in selectedLines)
//            yield return ExecuteLine(line);
//    }

//    private void SetDialogueOptions(string option1, string option2)
//    {
//        cacheLastOption1.GetComponent<Button>().GetComponentInChildren<TextMeshProUGUI>().text = option1;
//        cacheLastOption2.GetComponent<Button>().GetComponentInChildren<TextMeshProUGUI>().text = option2;
//    }

//    private IEnumerator WaitForOptionSelection()
//    {
//        selectedOption = -1;
//        bool optionConfirmed = false;
//        while (!optionConfirmed)
//        {
//            WobbleIndicatorAtOption();
//            if (IsPointerOverUIObject(cacheLastOption1) && Input.GetMouseButtonDown(0)) { selectedOption = 0; optionConfirmed = true; }
//            else if (IsPointerOverUIObject(cacheLastOption2) && Input.GetMouseButtonDown(0)) { selectedOption = 1; optionConfirmed = true; }
//            yield return null;
//        }
//    }

//    private bool IsPointerOverUIObject(GameObject option)
//    {
//        PointerEventData eventData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
//        var results = new List<RaycastResult>();
//        EventSystem.current.RaycastAll(eventData, results);
//        return results.Any(result => result.gameObject == option);
//    }

//    private IEnumerator TypeText(string text)
//    {
//        TextMeshProUGUI dialogueText = speechBubbleGO.GetComponentInChildren<TextMeshProUGUI>();
//        dialogueText.text = "";
//        float currentTypingSpeed = typingSpeed;
//        foreach (char letter in text)
//        {
//            dialogueText.text += letter;
//            yield return new WaitForSeconds(Input.GetMouseButton(0) ? fastTypingSpeed : currentTypingSpeed);
//        }
//    }

//    public void EndDialogue()
//    {
//        if (speechBubbleGO != null) { Destroy(speechBubbleGO); StopAllCoroutines(); }
//    }

//    private void WobbleIndicatorAtOption()
//    {
//        if (IsPointerOverUIObject(cacheLastOption1)) DialogueOptionIndicator.transform.position = cacheLastOption1.transform.position;
//        else if (IsPointerOverUIObject(cacheLastOption2)) DialogueOptionIndicator.transform.position = cacheLastOption2.transform.position;
//    }
//}