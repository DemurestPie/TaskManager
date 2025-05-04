using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectApiController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProjectApiController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        // GET: api/project
        // Returns a list of all projects
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var projects = await _db.Projects
                .Select(p => new
                {
                    p.Id,
                    p.Title,
                    p.Description,
                    p.Status,
                    p.Priority,
                    p.CreatedAt,
                    p.UpdatedAt,
                    p.DueDate,
                })
                .ToListAsync();

            return Ok(projects);
        }

        // GET: api/project/{id}
        // Returns a specific project by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var project = await _db.Projects
                .Include(p => p.User)
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        // POST: api/project/create
        // Creates a new project
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _db.Projects.AddAsync(project);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
        }

        // PUT: api/project/update/{id}
        // Updates an existing project
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] Project updatedProject)
        {
            if (id != updatedProject.Id)
            {
                return BadRequest("Project ID mismatch.");
            }

            var project = await _db.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            project.Title = updatedProject.Title;
            project.Description = updatedProject.Description;
            project.Status = updatedProject.Status;
            project.Priority = updatedProject.Priority;
            project.DueDate = updatedProject.DueDate;

            _db.Projects.Update(project);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/project/delete/{id}
        // Deletes a project by ID
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _db.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _db.Projects.Remove(project);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/projectapi/assign/{id}
        // Assigns a task to a project
        [HttpPost("assign/{id}")]
        public async Task<IActionResult> Assign(int id, [FromQuery] int taskId)
        {
            var project = await _db.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            var task = await _db.Tasks.FindAsync(taskId);
            if (task == null)
            {
                return NotFound();
            }

            project.Tasks.Add(task);
            await _db.SaveChangesAsync();

            return NoContent();
        }

    }
}
