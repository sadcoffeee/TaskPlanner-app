using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MyNamespace;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;
    public List<Task> TimedTasks = new List<Task>();
    public List<Task> IndefiniteTasks = new List<Task>();

    private string saveFilePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        saveFilePath = Path.Combine(Application.persistentDataPath, "REALtasks.json");
        LoadTasksFromJson();

        GameObject.Find("ScriptHolder").GetComponent<DailyTaskDisplayer>().DisplayTimedTasks();
    }

    public void AddTask(Task task)
    {
        if (task.Indefinite)
        {
            IndefiniteTasks.Add(task);
        }
        else
        {
            TimedTasks.Add(task);
        }

        SaveTasksToJson(); // Save the updated task lists to JSON
        Debug.Log($"Task '{task.Name}' added to {(task.Indefinite ? "IndefiniteTasks" : "TimedTasks")}.");
    }

    public void RemoveTask(Task task)
    {
        if (task.IsRecurring && !task.Indefinite)
        {
            // Calculate the new due date for recurring tasks
            DateTime newDueDate = task.DueDate.AddDays(task.Interval);

            Task newTask = new Task(
                task.Name,
                task.Description,
                newDueDate,
                task.IsRecurring,
                task.Interval
            );

            TimedTasks.Add(newTask);
            Debug.Log($"Recurring task '{task.Name}' updated with new due date: {newDueDate}.");
        }

        // Remove the task from the appropriate list
        if (task.Indefinite)
        {
            if (IndefiniteTasks.Remove(task))
            {
                Debug.Log($"Task '{task.Name}' removed from IndefiniteTasks.");
            }
            else
            {
                Debug.LogWarning($"Task '{task.Name}' not found in IndefiniteTasks.");
            }
        }
        else
        {
            if (TimedTasks.Remove(task))
            {
                Debug.Log($"Task '{task.Name}' removed from TimedTasks.");
            }
            else
            {
                Debug.LogWarning($"Task '{task.Name}' not found in TimedTasks.");
            }
        }

        SaveTasksToJson(); // Save the updated task lists to JSON
    }

    private void OnApplicationQuit()
    {
        SaveTasksToJson();
    }

    public void LoadTasksFromJson()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            List<Task> loadedTasks = JsonUtility.FromJson<TaskList>(json).Tasks;

            TimedTasks.Clear();
            IndefiniteTasks.Clear();

            foreach (var task in loadedTasks)
            {
                if (task.Indefinite)
                    IndefiniteTasks.Add(task);
                else
                    TimedTasks.Add(task);
            }

            Debug.Log("Tasks successfully loaded and sorted.");
        }
        else
        {
            Debug.LogWarning("No tasks.json file found.");
        }
    }


    public void SaveTasksToJson()
    {
        List<Task> allTasks = new List<Task>();
        allTasks.AddRange(TimedTasks);
        allTasks.AddRange(IndefiniteTasks);

        TaskList taskList = new TaskList { Tasks = allTasks };
        string json = JsonUtility.ToJson(taskList, true);
        File.WriteAllText(saveFilePath, json);

        Debug.Log("Tasks successfully saved.");
    }


    [Serializable]
    public class TaskList
    {
        public List<Task> Tasks;
    }
}
