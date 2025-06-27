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
    [SerializeField] private TMP_Dropdown yearDropdown;  // Dropdown for year
    [SerializeField] private TMP_Dropdown monthDropdown; // Dropdown for month
    [SerializeField] private TMP_Dropdown dayDropdown;   // Dropdown for day

    string taskTitle;
    string taskDescription;
    int taskYear;
    int taskMonth;
    int taskDay;
    bool taskRecurrence;
    int taskRecurrenceInt;
    bool taskIndefinite;

    public void updateTaskTitle(string title)
    {
        taskTitle = title;
    }

    public void updateTaskDescription(string desc)
    {
        taskDescription = desc;
    }

    public void updateTaskYear(int year)
    {
        taskYear = DateTime.Now.Year + year;
    }

    public void updateTaskMonth(int month)
    {
        taskMonth = month + 1;
    }

    public void updateTaskDay(int day)
    {
        taskDay = day + 1;
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
        DateTime today = DateTime.Now;
        taskYear = today.Year;
        taskMonth = today.Month;
        taskDay = today.Day;

        // Set dropdown values
        yearDropdown.value = taskYear - DateTime.Now.Year;
        monthDropdown.value = taskMonth - 1;
        dayDropdown.value = taskDay - 1;
    }

    public void buildTask()
    {
        DateTime taskDate;
        if (taskYear != 0 && taskMonth != 0 && taskDay != 0)
        {
            taskDate = new DateTime(taskYear, taskMonth, taskDay);
        }
        else
        {
            taskDate = DateTime.Now;
        }
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
            GetComponent<DailyTaskDisplayer>().DisplayTimedTasks(DateTime.Today);
        }
    }
}
