using Microsoft.AspNetCore.Mvc;
using PCLwebb.Models;

namespace PCLwebb.Controllers
{
    public class ClientController : Controller
    {
        public ModelContext context;
        public ClientController(ModelContext context) 
        { 
            this.context = context;
        }

        [HttpGet]
        public IActionResult AllClients()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            IQueryable<User> clientList = from user in context.Users select user;
            List<Client> clients = clientList.Where(user => user.UserName == User.Identity.Name).SelectMany(u => u.Clients).ToList();

            return View(clients);
        }

        [HttpGet]
        public IActionResult AddClient()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddClient(string clientName, string clientCity, string clientEmail)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            IQueryable<User> userList = from user in context.Users select user;
            userList = userList.Where(user => user.UserName == User.Identity.Name);
            User theUser = userList.FirstOrDefault();

            Client newClient = new Client();
            newClient.Name = clientName;
            newClient.City = clientCity;
            newClient.Email = clientEmail;
            newClient.CreatedBy = theUser.Id;

            context.Clients.Add(newClient);
            context.SaveChanges();

            return RedirectToAction("AllClients");
        }

        [HttpGet]
        public IActionResult ClientInfo(int clientID)
        {
            IQueryable<Client> clientList = from client in context.Clients select client;
            Client theClient = clientList.Where(client => client.Creator.UserName == User.Identity.Name && client.Id == clientID).FirstOrDefault();

            return View(theClient);
        }
    }
}
