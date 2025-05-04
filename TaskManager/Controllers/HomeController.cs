using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITaskRepository _taskRepo;
        private readonly IProjectRepository _projectRepo;

        public HomeController(ILogger<HomeController> logger, ITaskRepository taskRepo, IProjectRepository projectRepo)
        {
            _logger = logger;
            _taskRepo = taskRepo;
            _projectRepo = projectRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboardData()
        {
            var taskStats = await _taskRepo.GetAllTasksAsync();
            var projectStats = await _projectRepo.GetAllProjectsAsync();

            var result = new
            {
                TasksToDo = taskStats.Count(t => t.Status == TaskItemStatus.ToDo),
                TasksDone = taskStats.Count(t => t.Status == TaskItemStatus.Done),
                ProjectsToDo = projectStats.Count(p => p.Status == TaskItemStatus.ToDo),
                ProjectsDone = projectStats.Count(p => p.Status == TaskItemStatus.Done),
                RecentTasks = taskStats.OrderByDescending(t => t.CreatedAt).Take(5).Select(t => new
                {
                    t.Id,
                    t.Title,
                    Status = t.Status.ToString(),
                    Priority = t.Priority.ToString(),
                    User = t.User?.Name
                })
            };

            return Json(result);
        }
    }
}
