using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class TaskItem
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Status is required")]
        public TaskItemStatus Status { get; set; } // Enum for task status (e.g., ToDo, InProgress, Done)

        [Required(ErrorMessage = "Priority is required")]
        public TaskItemPriority Priority { get; set; } // Enum for task priority (e.g., Low, Medium, High)

        [DataType(DataType.Date)]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Default to current date

        [DataType(DataType.Date)]
        [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now; // Default to current date

        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateTime? DueDate { get; set; } // Nullable to allow for no due date
        public string? UserId { get; set; } // User ID of the task owner
        public User? User { get; set; } // Navigation property to the User who owns the task
        public int? ProjectId { get; set; } // Project ID if the task is associated with a project
        public Project? Project { get; set; } // Navigation property to the Project if applicable
    }
}
