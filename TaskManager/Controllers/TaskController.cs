using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ITaskRepository _taskRepo;
        private readonly UserManager<User> _userManager;

        public TaskController(UserManager<User> userManager, ITaskRepository taskRepo)
        {
            _taskRepo = taskRepo;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _taskRepo.GetAllTasksAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var task = await _taskRepo.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            return View(task);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var task = await _taskRepo.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TaskItem updatedTask)
        {
            if (id != updatedTask.Id) return BadRequest();

            await _taskRepo.UpdateTaskAsync(updatedTask);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var task = await _taskRepo.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            return View(task);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _taskRepo.GetTaskByIdAsync(id);

            if (task == null) return NotFound();

            await _taskRepo.DeleteTaskAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
