using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TaskController(ApplicationDbContext db)
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
                .Select(t => new {
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

    }
}
