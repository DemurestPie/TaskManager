using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public enum TaskItemStatus
    {
        [Display(Name = "To Do")]
        ToDo,
        [Display(Name = "In Progress")]
        InProgress,
        [Display(Name = "Done")]
        Done
    }

    public enum TaskItemPriority
    {
        Low,
        Medium,
        High
    }
}
