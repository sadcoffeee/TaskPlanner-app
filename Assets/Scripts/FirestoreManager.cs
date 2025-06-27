/*
using System;
using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine;
using MyNamespace;
using Firebase.Extensions;
using System.Linq;

public class FirestoreManager : MonoBehaviour
{
    
    private FirebaseFirestore db;

    private void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
    }

    public void SaveTaskToFirestore(Task task)
    {
        var docRef = db.Collection("ScheduledTasks").Document(task.Name);
        Dictionary<string, object> taskData = new Dictionary<string, object>
        {
            { "description", task.Description },
            { "dueDate", Timestamp.FromDateTime(task.DueDate.ToUniversalTime()) }, // Convert DateTime to Firestore Timestamp
            { "isRecurring", task.IsRecurring },
            { "name", task.Name },
            { "recurrenceFreq", task.Recurrence.Frequency.ToString() },
            { "recurrenceInt", task.Recurrence.Interval }
        };

        docRef.SetAsync(taskData).ContinueWithOnMainThread(t =>
        {
            if (t.IsCompleted)
            {
                Debug.Log($"Task {task.Name} saved to Firestore!");
            }
            else
            {
                Debug.LogError($"Failed to save task {task.Name} to Firestore. Exception: {t.Exception}");
            }
        });
    }

    public void LoadTasksFromFirestore(Action<List<Task>> onTasksLoaded)
    {
        if (db == null)
        {
            Debug.LogError("Firestore is not initialized!");
            return;
        }

        db.Collection("ScheduledTasks").GetSnapshotAsync().ContinueWithOnMainThread(t =>
        {
            Debug.Log($"Task Status: Faulted={t.IsFaulted}, Canceled={t.IsCanceled}, Completed={t.IsCompleted}");
            if (t.IsFaulted || t.IsCanceled)
            {
                Debug.LogError($"Failed to load tasks from Firestore. Exception: {t.Exception}");
                return;
            }

            QuerySnapshot snapshot = t.Result;
            Debug.Log($"Snapshot Status: Document Count={snapshot.Documents.Count()}");

            List<Task> loadedTasks = new List<Task>();

            foreach (var document in snapshot.Documents)
            {
                try
                {
                    Dictionary<string, object> taskData = document.ToDictionary();

                    // Convert Firestore Timestamp back to DateTime
                    Timestamp firestoreTimestamp = (Timestamp)taskData["dueDate"];
                    DateTime dueDate = firestoreTimestamp.ToDateTime();

                    Task task = new Task(
                        (string)taskData["name"],
                        (string)taskData["description"],
                        dueDate, // Use the converted DateTime
                        (bool)taskData["isRecurring"],
                        new RecurrenceType(
                            Enum.Parse<RecurrenceFrequency>((string)taskData["recurrenceFreq"]),
                            Convert.ToInt32(taskData["recurrenceInt"])
                        )
                    );
                    loadedTasks.Add(task);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error parsing task document: {document.Id}. Exception: {ex.Message}");
                }
            }

            Debug.Log($"Loaded Tasks Count: {loadedTasks.Count}");
            onTasksLoaded?.Invoke(loadedTasks);
        });
    }

    public void DeleteTaskFromFirestore(Task task)
    {
        var docRef = db.Collection("ScheduledTasks").Document(task.Name);
        docRef.DeleteAsync().ContinueWithOnMainThread(t =>
        {
            if (t.IsCompleted)
            {
                Debug.Log($"Task {task.Name} deleted from Firestore.");
            }
            else
            {
                Debug.LogError($"Failed to delete task {task.Name} from Firestore. Exception: {t.Exception}");
            }
        });
    }
    
}
*/
