using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class TaskManager : MonoBehaviour
{
    public List<AbstractTask> AssignedTasks = new List<AbstractTask>();
    public UnityEvent<string> OnFlagUpdated;

    private void Awake()
    {
        if (OnFlagUpdated == null)
        {
            OnFlagUpdated = new UnityEvent<string>();
        }
    }

    public void AddTask(AbstractTask task)
    {
        AssignedTasks.Add(task);
        task.OnAdd();
    }

    public void AddTaskByName(string taskName)
    {
        var namedTask = new SimpleTask(taskName);
        AssignedTasks.Add(namedTask);
    }

    public void CompleteTask(AbstractTask task)
    {
        var existingTask = AssignedTasks.FirstOrDefault(x => x == task);
        if (existingTask != null)
        {
            existingTask.IsCompleted = true;
            OnFlagSet(existingTask.Name);
        }
    }

    public void CompleteTaskByName(string taskName)
    {
        var existingTask = AssignedTasks.FirstOrDefault(x => x.Name == taskName);
        if (existingTask != null)
        {
            existingTask.IsCompleted = true;
            OnFlagSet(taskName);
        }
    }

    public bool CheckTaskCompletion(string taskName)
    {
        var existingTask = AssignedTasks.FirstOrDefault(x => x.Name == taskName);
        return existingTask != null ? existingTask.IsCompleted : false;
    }

    private void OnFlagSet(string flag)
    {
        OnFlagUpdated.Invoke(flag);
    }
}

public abstract class AbstractTask
{
    public string Name = string.Empty;
    public bool IsCompleted = false;

    public virtual void OnAdd()
    {

    }

    public virtual void OnRemove()
    {

    }

    public virtual void OnComplete()
    {
        IsCompleted = true;
    }
}

public class SimpleTask : AbstractTask
{
    public SimpleTask(string name)
    {
        this.Name = name;
    }
}

public class TestFetchTask : AbstractTask
{
    public GameObject objectToFetch;
    public GameObject spawnedFetchObject;
    private Transform parent;

    public TestFetchTask(Transform parent)
    {
        this.parent = parent;
    }

    public override void OnAdd()
    {
        if (objectToFetch == null)
        {
            objectToFetch = (GameObject)Resources.Load("Tasks/test");
        }

        Debug.Log("TestFetchTask was added!");

        spawnedFetchObject = GameObject.Instantiate(objectToFetch);
        spawnedFetchObject.transform.position = parent.transform.position;
    }

    public override void OnRemove()
    {
        GameObject.Destroy(spawnedFetchObject);
    }
}