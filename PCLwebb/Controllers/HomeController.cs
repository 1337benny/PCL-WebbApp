using Microsoft.AspNetCore.Mvc;
using PCLwebb.Models;
using System.Diagnostics;

namespace PCLwebb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ModelContext context;
        

        public HomeController(ILogger<HomeController> logger, ModelContext context)
        {
            _logger = logger;
            this.context = context;

        }

        public IActionResult Index()
        {
            IQueryable<User> userList = from user in context.Users select user;
            userList = userList.Where(user => user.UserName == User.Identity.Name);
            User theUser = userList.FirstOrDefault();
            

            return View(theUser);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
