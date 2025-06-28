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
    public GameObject dayTitlePrefab;


    public RectTransform taskCardsHolder; // Parent holder for task cards
    public TextMeshProUGUI taskCardsTitle;
    public Sprite trashcan;
    bool displayingTimedTasks = true;

    // Display tasks for a specific day
    public void DisplayTimedTasks()
    {
        ClearTaskCards();
        Debug.Log("Displaying all timed tasks");
        displayingTimedTasks = true;

        // Group tasks by date
        var groupedTasks = new Dictionary<DateTime, List<Task>>();

        foreach (Task task in TaskManager.Instance.TimedTasks)
        {
            DateTime date = task.DueDate.Date;

            if (!groupedTasks.ContainsKey(date))
                groupedTasks[date] = new List<Task>();

            groupedTasks[date].Add(task);
        }

        // Sort the dates closest to today first
        var sortedDates = new List<DateTime>(groupedTasks.Keys);
        sortedDates.Sort((a, b) => (a - DateTime.Today).Days.CompareTo((b - DateTime.Today).Days));

        taskCardsTitle.text = "Scheduled tasks";

        foreach (DateTime date in sortedDates)
        {
            // Create and format day title
            GameObject title = Instantiate(dayTitlePrefab, taskCardsHolder);
            title.GetComponentInChildren<TextMeshProUGUI>().text = FormatDateWithOrdinal(date);

            foreach (Task task in groupedTasks[date])
            {
                if (date < DateTime.Today)
                    CreateTaskCard(task, taskCards.late);
                else
                    CreateTaskCard(task, taskCards.timed);
            }
        }
    }

    public void DisplayIndefiniteTasks()
    {
        ClearTaskCards();
        Debug.Log("Displaying indefinite tasks");
        displayingTimedTasks = false;
        taskCardsTitle.text = "Untimed tasks";

        foreach (Task task in TaskManager.Instance.IndefiniteTasks)
        {
            CreateTaskCard(task, taskCards.indefinite);
        }
    }
    public void ToggleTaskDisplay()
    {
        if (displayingTimedTasks) DisplayIndefiniteTasks();
        else DisplayTimedTasks();
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
                            DisplayTimedTasks(); // Refresh the display for the day after removing the task

                        });
                    });
                }

                // Bind the completion button to remove the task
                taskCardInstance.transform.Find("CompleteTaskButton").GetComponent<Button>().onClick.AddListener(() =>
                {
                    TaskManager.Instance.RemoveTask(task);
                    DisplayTimedTasks(); // Refresh the display for the day after removing the task
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
                            DisplayTimedTasks(); // Refresh the display for the day after removing the task

                        });
                    });
                }
                // Bind the completion button to remove the task
                lateTaskCardInstance.transform.Find("CompleteTaskButton").GetComponent<Button>().onClick.AddListener(() =>
                {
                    TaskManager.Instance.RemoveTask(task);
                    DisplayTimedTasks();
                });
                break;

        }

    }
    // Helper method to format the date with an ordinal suffix
    private string FormatDateWithOrdinal(DateTime date)
    {
        if (date.Date == DateTime.Today.Date) return "Today";

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
