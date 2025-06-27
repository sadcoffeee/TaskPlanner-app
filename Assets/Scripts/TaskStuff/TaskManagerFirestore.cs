using System;
using System.Collections.Generic;
using UnityEngine;
using MyNamespace;
/*
public class TaskManagerFirestore : MonoBehaviour
{
    public static TaskManagerFirestore Instance;
    public List<Task> Tasks = new List<Task>();

    private FirestoreManager firestoreManager;

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

        // Initialize FirestoreManager
        firestoreManager = FindObjectOfType<FirestoreManager>();
        if (firestoreManager == null)
        {
            Debug.LogError("FirestoreManager is missing from the scene!");
            return;
        }
    }

    public void AddTask(Task task)
    {
        Tasks.Add(task);
        SaveTaskToFirestore(task); // Save the new task to Firestore immediately
    }

    public void RemoveTask(Task task)
    {
        if (task.IsRecurring && task.Recurrence != null)
        {
            DateTime newDueDate = task.DueDate;

            switch (task.Recurrence.Frequency)
            {
                case RecurrenceFrequency.Daily:
                    newDueDate = task.DueDate.AddDays(task.Recurrence.Interval);
                    break;

                case RecurrenceFrequency.Weekly:
                    newDueDate = task.DueDate.AddDays(task.Recurrence.Interval * 7);
                    break;

                // Add more recurrence types if needed
                default:
                    Debug.LogWarning("Unsupported recurrence type.");
                    break;
            }

            // Create a new task with the updated due date
            Task newTask = new Task(
                task.Name,
                task.Description,
                newDueDate,
                task.IsRecurring,
                task.Recurrence
            );

            // Add the new task to the list and Firestore
            Tasks.Add(newTask);
            SaveTaskToFirestore(newTask);
        }

        // Remove the completed task from the list and Firestore
        Tasks.Remove(task);
        DeleteTaskFromFirestore(task);
    }

    private void OnApplicationQuit()
    {
        SaveTasksToFirestore();
    }

    public void LoadTasksFromFirestore()
    {
        firestoreManager.LoadTasksFromFirestore((loadedTasks) =>
        {
            Tasks = loadedTasks;
            Debug.Log("Tasks loaded from Firestore.");
        });
        
    }

    private void SaveTasksToFirestore()
    {
        foreach (var task in Tasks)
        {
            SaveTaskToFirestore(task);
        }
        Debug.Log("All tasks saved to Firestore.");
    }

    private void SaveTaskToFirestore(Task task)
    {
        firestoreManager.SaveTaskToFirestore(task);
    }

    private void DeleteTaskFromFirestore(Task task)
    {
        firestoreManager.DeleteTaskFromFirestore(task);
    }
}
*/
