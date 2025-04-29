using System.ComponentModel.DataAnnotations;
using System.Data;

namespace TaskManager.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Project Name is required.")]
        public string Name { get; set; }
        public string? Description { get; set; }

        [Required(ErrorMessage = "Project Status is required.")]
        public TaskItemStatus Status { get; set; } // Enum for project status (e.g., Not Started, In Progress, Completed)

        [Required(ErrorMessage = "Project Priority is required.")]
        public TaskItemPriority Priority { get; set; } // Enum for project priority (e.g., Low, Medium, High)

        [Required(ErrorMessage = "Project Owner is required.")]
        public string UserId { get; set; } // User ID of the project owner
        public User User { get; set; } // Navigation property to the User model
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>(); // Navigation property to the TaskItem model
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [DataType(DataType.Date)]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; } = DateTime.Now.AddDays(7); // Default due date is 7 days from now

    }
}
