using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Calendar : MonoBehaviour
{
    public RectTransform calendarGrid; // Grid to hold day objects
    public GameObject dayPrefab;       // Prefab for a single day
    public TextMeshProUGUI monthYearText; // Text to show the current month and year
    public Color highlightColor;       // Color to highlight the current date

    private DateTime currentMonth;

    private DateTime selectedDate;
    private GameObject selectedDayButton;
    public Color defaultColor = Color.white; // Set this in the inspector to match unselected days


    void Start()
    {
        selectedDate = DateTime.Today;
        currentMonth = DateTime.Now;
        GenerateCalendar(currentMonth);
    }

    public void GenerateCalendar(DateTime month)
    {
        // Clear existing days
        foreach (Transform child in calendarGrid)
            Destroy(child.gameObject);

        // Update month/year text
        monthYearText.text = month.ToString("MMMM yyyy");

        // Determine first day and days in the month
        DateTime firstDay = new DateTime(month.Year, month.Month, 1);
        int daysInMonth = DateTime.DaysInMonth(month.Year, month.Month);

        // Calculate the day of the week (shift so Monday = 0, Sunday = 6)
        int startDayOfWeek = ((int)firstDay.DayOfWeek - 1 + 7) % 7;

        // Add placeholders for empty days
        for (int i = 0; i < startDayOfWeek; i++)
        {
            GameObject placeholder = Instantiate(dayPrefab, calendarGrid);
            placeholder.GetComponent<Button>().interactable = false;
            placeholder.GetComponent<Image>().color = new Color(0, 0, 0, 0); // Transparent
            placeholder.GetComponentInChildren<TextMeshProUGUI>().text = ""; // Empty
        }

        // Get today's date
        DateTime today = DateTime.Today;

        // Create a button for each day
        for (int day = 1; day <= daysInMonth; day++)
        {
            GameObject dayInstance = Instantiate(dayPrefab, calendarGrid);
            dayInstance.GetComponentInChildren<TextMeshProUGUI>().text = day.ToString();

            DateTime thisDay = new DateTime(month.Year, month.Month, day);
            Image dayImage = dayInstance.GetComponent<Image>();

            // Highlight if this is the selected date
            if (thisDay == selectedDate)
            {
                dayImage.color = highlightColor;
                selectedDayButton = dayInstance;
            }
            else
            {
                dayImage.color = defaultColor;
            }



            int dayCopy = day; // Prevent closure issue
            dayInstance.GetComponent<Button>().onClick.AddListener(() =>
            {
                // Reset previous highlight
                if (selectedDayButton != null)
                    selectedDayButton.GetComponent<Image>().color = defaultColor;

                // Update new selection
                selectedDate = new DateTime(month.Year, month.Month, dayCopy);
                selectedDayButton = dayInstance;
                selectedDayButton.GetComponent<Image>().color = highlightColor;

                // Set the taskDate
                transform.GetComponent<TaskBuilder>().taskDate = selectedDate;
            });
        }
    }

    public void NextMonth()
    {
        currentMonth = currentMonth.AddMonths(1);
        GenerateCalendar(currentMonth);
    }

    public void PreviousMonth()
    {
        currentMonth = currentMonth.AddMonths(-1);
        GenerateCalendar(currentMonth);
    }
}
