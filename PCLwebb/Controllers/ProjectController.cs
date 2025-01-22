using Microsoft.AspNetCore.Mvc;
using PCLwebb.Models;

namespace PCLwebb.Controllers
{
    public class ProjectController : Controller
    {
        public ModelContext context;

        public ProjectController(ModelContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult AllProjects()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            IQueryable<Project> projectList = from p in context.Projects select p;
            List<Project> projects = projectList.Where(p => p.Creator.UserName == User.Identity.Name).ToList();
            return View(projects);
        }

        [HttpGet]
        public IActionResult AddProject()
        {
            return View();
        }
    }
}
