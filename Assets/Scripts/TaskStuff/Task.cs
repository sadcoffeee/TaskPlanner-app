using System;

namespace MyNamespace
{
    [Serializable]
    public class Task
    {
        public string Name;
        public string Description;
        public string DueDateString;
        public bool IsRecurring;
        public int Interval;
        public bool Indefinite;

        // Property to work with DateTime directly
        public DateTime DueDate
        {
            get
            {
                if (Indefinite)
                    throw new InvalidOperationException("Indefinite tasks do not have a due date.");
                return DateTime.Parse(DueDateString);
            }
            set
            {
                if (Indefinite)
                    throw new InvalidOperationException("Cannot set a due date for an indefinite task.");
                DueDateString = value.ToString("o"); // ISO 8601 format
            }
        }

        public Task(string name, string description, DateTime? dueDate, bool isRecurring, int interval, bool indefinite = false)
        {
            Name = name;
            Description = description;
            IsRecurring = isRecurring;
            Interval = interval;
            Indefinite = indefinite;

            if (!indefinite && dueDate.HasValue)
            {
                DueDate = dueDate.Value;
            }
            else
            {
                DueDateString = null; // No due date for indefinite tasks
            }
        }
    }
}
