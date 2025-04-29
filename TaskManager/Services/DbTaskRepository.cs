using Microsoft.EntityFrameworkCore;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Services
{
    public class DbTaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _taskRepo;

        public DbTaskRepository(ApplicationDbContext taskRepo)
        {
            _taskRepo = taskRepo;
        }

        public async Task<IEnumerable<TaskItem?>> GetAllTasksAsync()
        {
            return await _taskRepo.Tasks
                .Include(t => t.User)
                .Include(t => t.Project)
                .ToListAsync();
        }

        public async Task<TaskItem?> GetTaskByIdAsync(int id)
        {
            return await _taskRepo.Tasks.FindAsync(id);
        }

        public async Task AddTaskAsync(TaskItem task)
        {
            await _taskRepo.Tasks.AddAsync(task);
            await _taskRepo.SaveChangesAsync();
        }

        public async Task UpdateTaskAsync(TaskItem task)
        {
            _taskRepo.Tasks.Update(task);
            await _taskRepo.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(int id)
        {
            var task = await _taskRepo.Tasks.FindAsync(id);
            if (task != null)
            {
                _taskRepo.Tasks.Remove(task);
                await _taskRepo.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TaskItem>> GetTasksByUserIdAsync(string userId)
        {
            return await _taskRepo.Tasks.Where(t => t.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetTasksByProjectIdAsync(int projectId)
        {
            return await _taskRepo.Tasks.Where(t => t.ProjectId == projectId).ToListAsync();
        }
    }
}
