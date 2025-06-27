using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MyNamespace;

public class DailyTaskDisplayer : MonoBehaviour
{
    public GameObject taskCardPrefab; // Task card prefab
    public GameObject lateTaskCardPrefab; 
    public GameObject indefiniteTaskCardPrefab;
    public GameObject descTaskCardPrefab;
    public GameObject descIndefiniteTaskCardPrefab;

    public RectTransform taskCardsHolder; // Parent holder for task cards
    public TextMeshProUGUI taskCardsTitle;
    public Sprite trashcan;

    // Display tasks for a specific day
    public void DisplayTimedTasks(DateTime whichDay)
    {
        ClearTaskCards();
        Debug.Log("Displaying tasks for " + whichDay);
        taskCardsTitle.text = FormatDateWithOrdinal(whichDay);

        foreach (Task task in TaskManager.Instance.TimedTasks)
        {
            // Check if the task's date matches the selected date (ignoring the time part)
            if (task.DueDate.Date == whichDay.Date)
            {
                CreateTaskCard(task, taskCards.timed);
            }
            else if (task.DueDate.Date < DateTime.Today)
            {
                CreateTaskCard(task, taskCards.late);
            }
        }
    }
    public void DisplayTodaysTasks()
    {
        DisplayTimedTasks(DateTime.Now);
    }
    public void DisplayIndefiniteTasks()
    {
        ClearTaskCards();
        Debug.Log("Displaying indefinite tasks");
        taskCardsTitle.text = "Indefinite tasks";

        foreach (Task task in TaskManager.Instance.IndefiniteTasks)
        {
            CreateTaskCard(task, taskCards.indefinite);
        }
    }

    // Clear existing task cards from the UI
    private void ClearTaskCards()
    {
        foreach (Transform child in taskCardsHolder)
        {
            Destroy(child.gameObject);
        }
    }

    // Create a task card UI and bind the completion button
    private void CreateTaskCard(Task task, taskCards cardType)
    {
        switch(cardType)
        {
            case taskCards.timed:
                GameObject taskCardInstance;

                // set the description, if it exists
                if (!string.IsNullOrEmpty(task.Description))
                {
                    taskCardInstance = Instantiate(descTaskCardPrefab, taskCardsHolder);
                    taskCardInstance.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = task.Description;
                } else
                {
                    taskCardInstance = Instantiate(taskCardPrefab, taskCardsHolder);

                }

                // Populate the task card UI with task data
                taskCardInstance.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = task.Name;

                if (task.IsRecurring)
                {
                    GameObject icon = taskCardInstance.transform.Find("RecurrenceIcon").gameObject;
                    icon.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = task.Interval.ToString();
                    icon.SetActive(true);
                    taskCardInstance.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        icon.transform.GetChild(0).gameObject.SetActive(false);
                        icon.GetComponent<Image>().sprite = trashcan;
                        icon.AddComponent<Button>().onClick.AddListener(() =>
                        {
                            TaskManager.Instance.TimedTasks.Remove(task);
                            DisplayTimedTasks(task.DueDate); // Refresh the display for the day after removing the task

                        });
                    });
                }

                // Bind the completion button to remove the task
                taskCardInstance.transform.Find("CompleteTaskButton").GetComponent<Button>().onClick.AddListener(() =>
                {
                    TaskManager.Instance.RemoveTask(task);
                    DisplayTimedTasks(task.DueDate); // Refresh the display for the day after removing the task
                });
                break;
            case taskCards.indefinite:
                GameObject indefiniteTaskCardInstance;

                if (!string.IsNullOrEmpty(task.Description))
                {
                    indefiniteTaskCardInstance = Instantiate(descIndefiniteTaskCardPrefab, taskCardsHolder);
                    indefiniteTaskCardInstance.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = task.Description;
                }
                else
                {
                    indefiniteTaskCardInstance = Instantiate(indefiniteTaskCardPrefab, taskCardsHolder);
                }

                // Populate the task card UI with task data
                indefiniteTaskCardInstance.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = task.Name;

                // Bind the completion button to remove the task
                indefiniteTaskCardInstance.GetComponentInChildren<Button>().onClick.AddListener(() =>
                {
                    TaskManager.Instance.RemoveTask(task);
                    DisplayIndefiniteTasks(); 
                });
                break;
            case taskCards.late:
                GameObject lateTaskCardInstance = Instantiate(lateTaskCardPrefab, taskCardsHolder);

                // Populate the task card UI with task data
                lateTaskCardInstance.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = task.Name;

                int lateDays = (DateTime.Today - task.DueDate.Date).Days;
                lateTaskCardInstance.transform.Find("LateDays").GetComponent<TextMeshProUGUI>().text = $"{lateDays} day{(lateDays == 1 ? "" : "s")} late";

                if (task.IsRecurring)
                {
                    GameObject icon = lateTaskCardInstance.transform.Find("RecurrenceIcon").gameObject;
                    icon.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = task.Interval.ToString();
                    icon.SetActive(true);
                    lateTaskCardInstance.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        icon.transform.GetChild(0).gameObject.SetActive(false);
                        icon.GetComponent<Image>().sprite = trashcan;
                        icon.AddComponent<Button>().onClick.AddListener(() =>
                        {
                            TaskManager.Instance.TimedTasks.Remove(task);
                            DisplayTimedTasks(DateTime.Today); // Refresh the display for the day after removing the task

                        });
                    });
                }
                // Bind the completion button to remove the task
                lateTaskCardInstance.transform.Find("CompleteTaskButton").GetComponent<Button>().onClick.AddListener(() =>
                {
                    TaskManager.Instance.RemoveTask(task);
                    DisplayTimedTasks(DateTime.Now);
                });
                break;

        }

    }
    // Helper method to format the date with an ordinal suffix
    private string FormatDateWithOrdinal(DateTime date)
    {
        string dayWithSuffix = GetOrdinalSuffix(date.Day);
        return $"{date:MMMM} {dayWithSuffix}, {date:yyyy}";
    }

    // Helper method to get the ordinal suffix for a day
    private string GetOrdinalSuffix(int day)
    {
        if (day >= 11 && day <= 13) return day + "th"; // Special case for 11th, 12th, 13th
        return day + (day % 10 == 1 ? "st" : day % 10 == 2 ? "nd" : day % 10 == 3 ? "rd" : "th");
    }
    enum taskCards
    {
        timed, indefinite, late
    }
}
