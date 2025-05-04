using Microsoft.AspNetCore.Mvc;
using TaskManager.Extensions;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardApiController : ControllerBase
    {
        private readonly ITaskRepository _taskRepo;
        private readonly IProjectRepository _projectRepo;

        public DashboardApiController(ITaskRepository taskRepo, IProjectRepository projectRepo)
        {
            _taskRepo = taskRepo;
            _projectRepo = projectRepo;
        }

        // GET: api/dashboardapi
        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            var taskStats = await _taskRepo.GetAllTasksAsync();
            var projectStats = await _projectRepo.GetAllProjectsAsync();

            var result = new
            {
                TasksToDo = taskStats.Count(t => t.Status == TaskItemStatus.ToDo),
                TasksDone = taskStats.Count(t => t.Status == TaskItemStatus.Done),
                ProjectsToDo = projectStats.Count(p => p.Status == TaskItemStatus.ToDo),
                ProjectsDone = projectStats.Count(p => p.Status == TaskItemStatus.Done),
                RecentTasks = taskStats
                    .OrderByDescending(t => t.CreatedAt)
                    .Take(10)
                    .Select(t => new
                    {
                        t.Id,
                        t.Title,
                        Status = t.Status.GetDisplayName(),
                        Priority = t.Priority.GetDisplayName(),
                        User = t.User?.Name
                    })
            };

            return Ok(result);
        }
    }
}
