using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MyNamespace;

public class TaskBuilder : MonoBehaviour
{
    [SerializeField] private GameObject addTaskPanel; // Reference to the task creation panel

    string taskTitle;
    string taskDescription;
    bool taskRecurrence;
    int taskRecurrenceInt;
    bool taskIndefinite;
    public DateTime taskDate;


    public void updateTaskTitle(string title)
    {
        taskTitle = title;
    }

    public void updateTaskDescription(string desc)
    {
        taskDescription = desc;
    }

    public void updateTaskRecurrenceInterval(string interval)
    {
        taskRecurrenceInt = int.Parse(interval);
    }
    public void updateTaskIndefinite(bool yes)
    {
        taskIndefinite = yes;
    }
    public void InitializeTaskPanel()
    {
        // Activate the task creation panel
        addTaskPanel.SetActive(true);

        // Get the current date
        taskDate = DateTime.Now;

        taskRecurrenceInt = 0;
    }

    public void buildTask()
    {
        if(taskRecurrenceInt != 0) 
        {
            taskRecurrence = true;
        }

        TaskManager.Instance.AddTask(new Task(taskTitle, taskDescription, taskDate, taskRecurrence, taskRecurrenceInt, taskIndefinite));
        addTaskPanel.SetActive(false);
        if (taskIndefinite)
        {
            GetComponent<DailyTaskDisplayer>().DisplayIndefiniteTasks();
        } else
        {
            GetComponent<DailyTaskDisplayer>().DisplayTimedTasks();
        }
    }
}
