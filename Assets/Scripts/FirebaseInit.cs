/*
using Firebase;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseInit : MonoBehaviour
{
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            Firebase.DependencyStatus status = task.Result;
            if (status == Firebase.DependencyStatus.Available)
            {
                Debug.Log("Firebase initialized successfully.");
                TaskManager.Instance.LoadTasksFromFirestore();
            }
            else
            {
                Debug.LogError($"Could not resolve Firebase dependencies: {status}");
            }
        });
    }
}
*/
