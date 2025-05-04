using Microsoft.AspNetCore.Identity;

namespace TaskManager.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; } = string.Empty; // Default to empty string
        public RoleType Role { get; set; } = RoleType.User; // Default to User role
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Default to current date
        public DateTime UpdatedAt { get; set; } = DateTime.Now; // Default to current date
        public ICollection<TaskItem>? Tasks { get; set; }
        public ICollection<Project>? Projects { get; set; }


    }
}
