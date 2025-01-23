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
            IQueryable<Client> clientList = from c in context.Clients select c;
            List<Client> clients = clientList.Where(c => c.Creator.UserName == User.Identity.Name).ToList();
            return View(clients);
        }

        [HttpPost]
        public IActionResult AddProject(string projectName, string projectDescription, DateOnly projectStartdate, DateOnly projectEnddate, bool projectStatus, int clientID)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            IQueryable<User> userList = from user in context.Users select user;
            userList = userList.Where(user => user.UserName == User.Identity.Name);
            User theUser = userList.FirstOrDefault();

            Project project = new Project();
            project.Name = projectName;
            project.Description = projectDescription;
            project.StartDate = projectStartdate;
            project.EndDate = projectEnddate;
            project.IsActive = projectStatus;
            project.CreatedBy = theUser.Id;


            context.Projects.Add(project);
            context.SaveChanges();

            if (clientID > 0)
            {
                Client_Has_Project client_Has_Project = new Client_Has_Project();
                client_Has_Project.ProjectID = project.Id;
                client_Has_Project.ClientID = clientID;
                context.ClientHasProjects.Add(client_Has_Project);
                context.SaveChanges();
            }

            return RedirectToAction("AllProjects");
        }

        [HttpGet]
        public IActionResult ProjectInfo(int projectID)
        {
            IQueryable<Project> projectList = from p in context.Projects select p;
            Project project = projectList.Where(p => p.Creator.UserName == User.Identity.Name && p.Id == projectID).ToList().FirstOrDefault();
            return View(project);
        }

        [HttpPost]
        public IActionResult EditProjectInfo(Project updatedProject)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            IQueryable<User> userList = from user in context.Users select user;
            userList = userList.Where(user => user.UserName == User.Identity.Name);
            User theUser = userList.FirstOrDefault();

            IQueryable<Project> projectList = from p in context.Projects select p;
            Project theProject = projectList.Where(p => p.Creator.UserName == User.Identity.Name && p.Id == updatedProject.Id).ToList().FirstOrDefault();

            theProject.Name = updatedProject.Name;
            theProject.Description = updatedProject.Description;
            theProject.StartDate = updatedProject.StartDate;
            theProject.EndDate = updatedProject.EndDate;
            theProject.IsActive = updatedProject.IsActive;

            context.Projects.Update(theProject);
            context.SaveChanges();
            return RedirectToAction("ProjectInfo", new { projectID = theProject.Id });
        }
    }
}
