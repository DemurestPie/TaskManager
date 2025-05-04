using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskApiController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TaskApiController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        // GET: api/task
        // Returns a list of all tasks

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _db.Tasks
                .Include(t => t.User)
                .Select(t => new
                {
                    t.Id,
                    t.Title,
                    t.Status,
                    User = new
                    {
                        t.User.Id,
                        t.User.Name
                    }
                })
                .ToListAsync();

            return Ok(tasks);
        }


        // GET: api/task/{id}
        // Returns a specific task by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _db.Tasks
                .Include(t => t.User)
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            // Create a new anonymous object so it won't send user info
            var result = new
            {
                task.Id,
                task.Title,
                task.Description,
                task.Status,
                task.Priority,
                task.CreatedAt,
                task.UpdatedAt,
                task.DueDate,
                User = task.User != null ? new
                {
                    task.User.Id,
                    task.User.Name
                } : null,
                Project = task.Project != null ? new
                {
                    task.Project.Id,
                    task.Project.Title
                } : null
            };

            return Ok(result);
        }


        // POST: api/task/create
        // Creates a new task
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] TaskItem task)
        {
            if (task == null)
            {
                return BadRequest("Task cannot be null.");
            }

            await _db.Tasks.AddAsync(task);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        // PUT: api/task/update/{id}
        // Updates an existing task
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] TaskItem task)
        {
            if (id != task.Id)
            {
                return BadRequest("Task ID mismatch.");
            }

            var existingTask = await _db.Tasks.FindAsync(id);
            if (existingTask == null)
            {
                return NotFound();
            }

            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.Status = task.Status;
            existingTask.Priority = task.Priority;
            existingTask.DueDate = task.DueDate;

            _db.Tasks.Update(existingTask);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/task/delete/{id}
        // Deletes a specific task by ID
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _db.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _db.Tasks.Remove(task);
            await _db.SaveChangesAsync();

            return NoContent();
        }

    }
}
