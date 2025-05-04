using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Controllers.Api
{
    public class ProjectController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
