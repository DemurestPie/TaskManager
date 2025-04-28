using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskItem task)
        {
            task.UserId = _userManager.GetUserId(User);
            task.CreatedAt = DateTime.Now;
            task.UpdatedAt = DateTime.Now;

            if (ModelState.IsValid)
            {
                await _taskRepo.AddTaskAsync(task);
                return RedirectToAction("Index");
            }

            if(!ModelState.IsValid)
            {
                // Handle validation errors
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    // Log the error or display it to the user
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            return View(task);
        }

        public IActionResult Edit(int id)
        {
            // Retrieve the task from the database using the id
            TaskItem task = new TaskItem(); // Replace with actual retrieval logic
            return View(task);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(TaskItem task)
        {
            if (ModelState.IsValid)
            {
                // Update the task in the database
                // Redirect to the task list or details page
                return RedirectToAction("Index");
            }
            return View(task);
        }

        public IActionResult Delete(int id)
        {
            // Retrieve the task from the database using the id
            TaskItem task = new TaskItem(); // Replace with actual retrieval logic
            return View(task);
        }
    }
}
