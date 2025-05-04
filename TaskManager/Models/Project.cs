using System.ComponentModel.DataAnnotations;
using System.Data;

namespace TaskManager.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Project Status is required.")]
        public TaskItemStatus Status { get; set; } // Enum for project status (e.g., Not Started, In Progress, Completed)

        [Required(ErrorMessage = "Project Priority is required.")]
        public TaskItemPriority Priority { get; set; } // Enum for project priority (e.g., Low, Medium, High)
        public string? UserId { get; set; } // User ID of the project owner
        public User? User { get; set; } // Navigation property to the User model
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>(); // Navigation property to the TaskItem model
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [DataType(DataType.Date)]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; } 

    }
}
