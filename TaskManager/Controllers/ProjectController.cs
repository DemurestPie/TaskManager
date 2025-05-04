using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Extensions;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly IProjectRepository _projectRepo;
        private readonly ITaskRepository _taskRepo;
        private readonly UserManager<User> _userManager;

        public ProjectController(UserManager<User> userManager, IProjectRepository projectRepo, ITaskRepository taskRepo)
        {
            _projectRepo = projectRepo;
            _userManager = userManager;
            _taskRepo = taskRepo;
        }
        public async Task<IActionResult> Index(string status, string priority, string excludeDone)
        {
            var projects = await _projectRepo.GetAllProjectsAsync();

            // Adds status filter
            if (!string.IsNullOrEmpty(status) && Enum.TryParse<TaskItemStatus>(status, out var parsedStatus))
            {
                projects = projects.Where(p => p.Status == parsedStatus);
                ViewBag.SelectedStatus = status;
            }

            // Adds priority filter
            if (!string.IsNullOrEmpty(priority) && Enum.TryParse<TaskItemPriority>(priority, out var parsedPriority))
            {
                projects = projects.Where(p => p.Priority == parsedPriority);
                ViewBag.SelectedPriority = priority;
            }

            // Adds exclude done filter
            if (excludeDone == "on")
            {
                projects = projects.Where(p => p.Status != TaskItemStatus.Done);
                ViewBag.ExcludeDone = true;
            }

            return View(projects);
        }


        public async Task<IActionResult> Create()
        {
            // User must be either Admin or Manager to create a project
            if (!User.IsAdmin(_userManager) && !User.IsManager(_userManager))
            {
                return Forbid();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project project)
        {
            if (!ModelState.IsValid)
            {
                // Log errors for debugging
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(project);
            }

            project.UserId = _userManager.GetUserId(User);

            if (project.UserId == null)
            {
                ModelState.AddModelError("", "User ID is required.");
                return View(project);
            }
            await _projectRepo.AddProjectAsync(project);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var project = await _projectRepo.GetProjectByIdAsync(id);
            if (project == null) return NotFound();
            return View(project);
        }

        public async Task<IActionResult> Edit(int id)
        {
            // User must be either Admin or Manager to edit a project
            if (!User.IsAdmin(_userManager) || !User.IsManager(_userManager))
            {
                return Forbid();
            }
            var project = await _projectRepo.GetProjectByIdAsync(id);
            if (project == null) return NotFound();
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Project project)
        {
            if (ModelState.IsValid)
            {
                project.UpdatedAt = DateTime.Now;
                await _projectRepo.UpdateProjectAsync(project);
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        public async Task<IActionResult> Delete(int id)
        {
            // User must be an admin to delete projects
            if (!User.IsAdmin(_userManager))
            {
                return Forbid();
            }
            var project = await _projectRepo.GetProjectByIdAsync(id);
            if (project == null) return NotFound();
            return View(project);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _projectRepo.DeleteProjectAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Assign(int id)
        {
            // User must be either Admin or Manager to assign a project
            if (!User.IsInRole("Admin") || !User.IsInRole("Manager"))
            {
                return Forbid();
            }
            var tasks = await _taskRepo.GetAllTasksAsync();
            var result = tasks.Select(t => new
            {
                t.Id,
                t.Title,
                Status = t.Status.ToString(), // Convert enum to string
                UserName = t.User?.Name // Get user's name if available
            });

            return Json(result);
        }
    }
}
