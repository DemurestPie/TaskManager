namespace TaskManager.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; } // User ID of the project owner
        public User User { get; set; } // Navigation property to the User model
        public ICollection<TaskItem> Tasks { get; set; } // Collection of tasks associated with the project
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }
}
