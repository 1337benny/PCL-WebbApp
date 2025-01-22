using Microsoft.AspNetCore.Mvc;
using PCLwebb.Models;

namespace PCLwebb.Controllers
{
    public class ListTaskController : Controller
    {
        public ModelContext context;

        public ListTaskController(ModelContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public ActionResult AddListTask(string taskName, string taskDescription, int checklistId)
        {
            var checklist = context.Checklists.FirstOrDefault(cl => cl.Id == checklistId);
            if (checklist == null)
            {
                return NotFound("Checklistan kunde inte hittas.");
            }

            ListTask task = new ListTask
            {
                Name = taskName,
                Description = taskDescription,
                ChecklistID = checklist.Id
            };

            context.ListTasks.Add(task);
            context.SaveChanges();

            return RedirectToAction("EditChecklist", "Checklist", new { checklistID = checklistId });
        }

        [HttpPost]
        public ActionResult DeleteListTask(int taskID)
        {
            var theTask = context.ListTasks.FirstOrDefault(task => task.Id == taskID);

            context.ListTasks.Remove(theTask);
            context.SaveChanges();

            return RedirectToAction("EditChecklist", "Checklist", new { checklistID = theTask.ChecklistID });
        }

    }
}
