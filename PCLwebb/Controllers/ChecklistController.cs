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
            IQueryable<User> userList = from user in context.Users select user;
            List<Checklist> checklists = userList.Where(user => user.UserName == User.Identity.Name).SelectMany(u => u.Checklists).ToList();
            
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

    }
}
