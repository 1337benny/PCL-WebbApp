using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCLwebb.Models;

namespace PCLwebb.Controllers
{
    public class ChecklistController : Controller
    {
        private ModelContext context;
        
        public ChecklistController(ModelContext context) 
        { 
            this.context = context;
        }

        [HttpGet]
        public IActionResult AllChecklists()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            
            IQueryable<Checklist> allChecklists = from c in context.Checklists select c;
            List<Checklist> checklists = allChecklists.Where(c => c.Creator.UserName == User.Identity.Name && c.IsTemplate == true).ToList();
            return View(checklists);
        }

        [HttpGet]
        public IActionResult AddChecklist()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            Checklist checklist = new Checklist();

            return View(checklist);
        }

        [HttpPost]
        public IActionResult AddChecklist(Checklist newChecklist)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            IQueryable<User> userList = from user in context.Users select user;
            userList = userList.Where(user => user.UserName == User.Identity.Name);
            User theUser = userList.FirstOrDefault();

            newChecklist.CreatorID = theUser.Id;

            context.Checklists.Add(newChecklist);
            context.SaveChanges();

            return RedirectToAction("AllChecklists");
        }

        [HttpGet]
        public IActionResult EditChecklist(int checklistID)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            IQueryable<Checklist> checkLists = from check in context.Checklists select check;
            checkLists = checkLists.Where(c => c.Id == checklistID && c.Creator.UserName == User.Identity.Name);
            Checklist theChecklist = checkLists.FirstOrDefault();
            return View(theChecklist);
        }

        [HttpPost]
        public IActionResult UpdateChecklistBasic(Checklist checklist, int checklistId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                RedirectToAction("Index", "Home");
            }
            
                IQueryable<Checklist> checkLists = from check in context.Checklists select check;
                checkLists = checkLists.Where(c => c.Id == checklistId && c.Creator.UserName == User.Identity.Name);
                Checklist theChecklist = checkLists.FirstOrDefault();

                theChecklist.Name = checklist.Name;
                theChecklist.Description = checklist.Description;

                context.Checklists.Update(theChecklist);
                context.SaveChanges();
            return RedirectToAction("EditChecklist", new { checklistID = checklistId });

        }

        [HttpPost]
        public IActionResult DeleteChecklist(int checklistID)
        {
            if (!User.Identity.IsAuthenticated)
            {
                RedirectToAction("Index", "Home");
            }

            IQueryable<Checklist> checkLists = from check in context.Checklists select check;
            checkLists = checkLists.Where(c => c.Id == checklistID && c.Creator.UserName == User.Identity.Name);
            Checklist theChecklist = checkLists.FirstOrDefault();

            //Krashar om man försöker ta bort en checklista som har kopior
            context.Checklists.Remove(theChecklist);
            context.SaveChanges();

            return RedirectToAction("AllChecklists");
        }

        public IActionResult DeleteChecklistFromProject(int projectId, int checklistID)
        {
            if (!User.Identity.IsAuthenticated)
            {
                RedirectToAction("Index", "Home");
            }

            IQueryable<Checklist> checkLists = from check in context.Checklists select check;
            checkLists = checkLists.Where(c => c.Id == checklistID && c.Creator.UserName == User.Identity.Name);
            Checklist theChecklist = checkLists.FirstOrDefault();

            IQueryable<Project_Has_Checklist> phc = from p in context.ProjectHasChecklists select p;
            phc = phc.Where(c => c.ChecklistID == checklistID && c.ProjectID == projectId && c.Checklist.Creator.UserName == User.Identity.Name);
            Project_Has_Checklist thePhc = phc.FirstOrDefault();

            context.ProjectHasChecklists.Remove(thePhc);
            context.Checklists.Remove(theChecklist);
            context.SaveChanges();

            return RedirectToAction("ProjectInfo", "Project", new { projectID = projectId });
        }

    }
}
