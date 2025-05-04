using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManager.Extensions;
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
        public async Task<IActionResult> Index(string status, string priority, string excludeDone)
        {
            var tasks = await _taskRepo.GetAllTasksAsync();

            // Adds status filter
            if (!string.IsNullOrEmpty(status) && Enum.TryParse<TaskItemStatus>(status, out var parsedStatus))
            {
                tasks = tasks.Where(t => t.Status == parsedStatus);
                ViewBag.SelectedStatus = status;
            }

            // Adds priority filter
            if (!string.IsNullOrEmpty(priority) && Enum.TryParse<TaskItemPriority>(priority, out var parsedPriority))
            {
                tasks = tasks.Where(t => t.Priority == parsedPriority);
                ViewBag.SelectedPriority = priority;
            }

            // Adds exclude done filter
            if (excludeDone == "on")
            {
                tasks = tasks.Where(t => t.Status != TaskItemStatus.Done);
                ViewBag.ExcludeDone = true;
            }

            return View(tasks);
        }


        public async Task<IActionResult> Create()
        {
            // User must be either Admin or Manager to create a task
            if (!User.IsAdmin(_userManager) && !User.IsManager(_userManager))
            {
                return Forbid();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskItem task)
        {
            if (!ModelState.IsValid)
            {
                // Log the errors to the console for debugging
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(task);
            }

            task.UserId = _userManager.GetUserId(User);

            if (task.UserId == null)
            {
                ModelState.AddModelError("", "User ID is required.");
                return View(task);
            }
            await _taskRepo.AddTaskAsync(task);

            return RedirectToAction(nameof(Index));
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
            // Check if task id was changed
            if (id != updatedTask.Id) return BadRequest();

            await _taskRepo.UpdateTaskAsync(updatedTask);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            // User must be an admin to delete tasks
            if (!User.IsAdmin(_userManager))
            {
                return Forbid();
            }
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
