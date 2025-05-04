using TaskManager.Models;

namespace TaskManager.Services
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetAllTasksAsync();
        Task<TaskItem> GetTaskByIdAsync(int id);
        Task AddTaskAsync(TaskItem task);
        Task UpdateTaskAsync(TaskItem task);
        Task DeleteTaskAsync(int id);
        Task<IEnumerable<TaskItem>> GetTasksByUserIdAsync(string userId);
        Task<IEnumerable<TaskItem>> GetTasksByProjectIdAsync(int projectId);
    }
}
