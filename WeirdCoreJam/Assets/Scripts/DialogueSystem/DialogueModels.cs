using System;
using System.Collections.Generic;

[System.Serializable]
public class DialogueNode
{
    public int id;
    public string text;
    public List<DialogueOption> options;
    public int nextID;  // Use a sentinel value like -1 to signify null
    public string checkTaskName;
    public List<DialogueOption> checkTaskOptions;
    public string completeTask;
    public string addTask;
    [System.NonSerialized] public DialogueNode nextLine;
}


[System.Serializable]
public class DialogueOption
{
    public string text;
    public int responseID;
    [System.NonSerialized] public DialogueNode response;  // This will not be serialized in JSON
}

[System.Serializable]
public class DialogueContainer
{
    public List<DialogueNode> dialogueNodes;
}
