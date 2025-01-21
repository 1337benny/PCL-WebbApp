using Microsoft.AspNetCore.Mvc;

namespace PCLwebb.Models
{
    public class Validator : Controller
    {
        public ModelContext context { get; set; }
        public Validator(ModelContext context) 
        { 
            this.context = context;
        }
        public bool IsAuthenticated()
        {
            if (User.Identity.IsAuthenticated)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
