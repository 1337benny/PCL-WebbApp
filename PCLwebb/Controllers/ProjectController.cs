﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            List<Project> projects = projectList.Where(p => p.Creator.UserName == User.Identity.Name).OrderByDescending(p => p.IsActive).ToList();
            return View(projects);
        }

        [HttpGet]
        public IActionResult AddProject()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            IQueryable<Client> clientList = from c in context.Clients select c;
            List<Client> clients = clientList.Where(c => c.Creator.UserName == User.Identity.Name).ToList();
            return View(clients);
        }

        [HttpPost]
        public IActionResult AddProject(string projectName, string projectDescription, /*DateOnly projectStartdate, DateOnly projectEnddate,*/ bool projectStatus, int clientID)
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
            //project.StartDate = projectStartdate;
            //project.EndDate = projectEnddate;
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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
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
            theProject.IsActive = updatedProject.IsActive;

            context.Projects.Update(theProject);
            context.SaveChanges();
            return RedirectToAction("ProjectInfo", new { projectID = theProject.Id });
        }

        [HttpPost]
        public IActionResult AddChecklistToProject(int projectId, int checklistID)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            // Hämta originalchecklistan med dess uppgifter
            var original = context.Checklists
                .Include(cl => cl.ListTasks)
                .FirstOrDefault(cl => cl.Id == checklistID);

            if (original == null)
            {
                return NotFound("Checklist not found");
            }

            // Skapa kopia av checklistan
            var checklistCopy = new Checklist
            {
                Name = original.Name,
                Description = original.Description,
                IsTemplate = false, // Kopian är inte en mall
                CreatorID = original.CreatorID,
                ParentChecklistID = original.Id,
                ListTasks = original.ListTasks.Select(task => new ListTask
                {
                    Name = task.Name,
                    Description = task.Description,
                    IsCompleted = false, // Alla tasks börjar som oavslutade
                    ChecklistID = null // Se till att kopplingen till originalets ID tas bort
                }).ToList()
            };

            // Lägg till checklistkopian i databasen
            context.Checklists.Add(checklistCopy);
            context.SaveChanges(); // Sparar kopian så att dess ID genereras

            // Hämta projektet
            var project = context.Projects.FirstOrDefault(p => p.Id == projectId);
            if (project == null)
            {
                return NotFound("Project not found");
            }

            // Koppla checklistkopian till projektet
            context.ProjectHasChecklists.Add(new Project_Has_Checklist
            {
                ProjectID = project.Id,
                ChecklistID = checklistCopy.Id // Nu har checklistCopy ett ID
            });

            // Spara ändringarna
            context.SaveChanges();

            // Omdirigera användaren till projektets informationssida
            return RedirectToAction("ProjectInfo", new { projectID = projectId });
        }



        [HttpGet]
        public IActionResult EditProjectChecklist(int checklistID)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            
            Checklist checklist = context.Checklists
                .Include(c => c.ListTasks) 
                .FirstOrDefault(c => c.Id == checklistID && c.Creator.UserName == User.Identity.Name);
            return View(checklist);
        }

        [HttpPost]
        public IActionResult UpdateProjectChecklist(Checklist checklist)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            // Hämta den befintliga checklistan från databasen
            var existingChecklist = context.Checklists
                .Include(c => c.ListTasks) // Ladda ListTasks
                .FirstOrDefault(c => c.Id == checklist.Id);

            if (existingChecklist != null)
            {
                // Uppdatera varje task i checklistan
                foreach (var updatedTask in checklist.ListTasks)
                {
                    var existingTask = existingChecklist.ListTasks
                        .FirstOrDefault(t => t.Id == updatedTask.Id);

                    if (existingTask != null)
                    {
                        existingTask.Assessment = updatedTask.Assessment;
                        existingTask.IsCompleted = updatedTask.IsCompleted;
                        existingTask.Note = updatedTask.Note;
                        existingTask.EditedDate = updatedTask.EditedDate;

                        context.ListTasks.Update(existingTask);
                    }
                }
                context.Checklists.Update(existingChecklist);

                // Spara ändringar till databasen
                context.SaveChanges();

                // Återvänd till en bekräftelsesida eller samma sida
                return RedirectToAction("EditProjectChecklist", new { checklistID = checklist.Id });
            }
            else
            {
                // Hantera om checklistan inte hittas
                ModelState.AddModelError("", "Checklistan kunde inte hittas.");
                return RedirectToAction("EditProjectChecklist", new { checklistID = checklist.Id });
            }
        }

        [HttpPost]
        public IActionResult DeleteProject(int projectId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            Project project = context.Projects.FirstOrDefault(p => p.Id == projectId && p.Creator.UserName == User.Identity.Name);

            List<Project_Has_Checklist> phc = project.ProjectHasChecklists.ToList();

            List<Client_Has_Project> chp = project.ClientHasProjects.ToList();

            foreach (Client_Has_Project client_Has_Project in chp)
            {
                context.Remove(client_Has_Project);
                
            }


            foreach (Project_Has_Checklist project_Has_Checklist in phc)
            {
                foreach (ListTask task in project_Has_Checklist.Checklist.ListTasks)
                {
                    context.ListTasks.Remove(task);
                    
                }

                context.Checklists.Remove(project_Has_Checklist.Checklist);
                context.ProjectHasChecklists.Remove(project_Has_Checklist);
                context.SaveChanges();
                
            }

            context.Projects.Remove(project);
            context.SaveChanges();

            return RedirectToAction("AllProjects");
        }

       


    }
}
