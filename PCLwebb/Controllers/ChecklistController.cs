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

    }
}
