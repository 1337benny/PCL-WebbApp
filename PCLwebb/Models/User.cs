using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PCLwebb.Models
{
    public class User : IdentityUser
    {
        public User()
        {

        }

        public string Firstname { get; set; }

        public string Lastname { get; set; }


        public DateOnly BirthDay { get; set; }

        public string? ProfilePicturePath { get; set; } = "/images/4a7de5d7-70f5-4a2a-9ef1-f691e98a4c38.jpeg";


        public bool IsActive { get; set; } = true;

        public virtual ICollection<Client> Clients { get; set; }

        public virtual ICollection<Checklist> Checklists { get; set; }

        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    }
}
