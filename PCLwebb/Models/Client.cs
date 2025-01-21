using System.ComponentModel.DataAnnotations.Schema;

namespace PCLwebb.Models
{
    public class Client
    {
        public Client() { }
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string? Email { get; set; }

        public string CreatedBy { get; set; }

        public virtual IEnumerable<Client_Has_Project> ClientHasProjects { get; set; } = new List<Client_Has_Project>();

        [ForeignKey(nameof(CreatedBy))]
        public virtual User Creator { get; set; }
    }
}
