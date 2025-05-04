using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Controllers.Api
{
    public class ProjectApiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
