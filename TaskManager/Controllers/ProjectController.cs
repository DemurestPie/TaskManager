using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly IProjectRepository _projectRepo;
        private readonly UserManager<User> _userManager;

        public ProjectController(UserManager<User> userManager, IProjectRepository projectRepo)
        {
            _projectRepo = projectRepo;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _projectRepo.GetAllProjectsAsync());
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project project)
        {
            if (!ModelState.IsValid)
            {
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
            var project = await _projectRepo.GetProjectByIdAsync(id);
            if (project == null) return NotFound();
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Project updatedProject)
        {
            if (id != updatedProject.Id) return BadRequest();

            await _projectRepo.UpdateProjectAsync(updatedProject);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var project = await _projectRepo.GetProjectByIdAsync(id);
            if (project == null) return NotFound();
            return View(project);
        }
    }
}
