using System.ComponentModel.DataAnnotations.Schema;

namespace PCLwebb.Models
{
    public class Client_Has_Project
    {
        public Client_Has_Project() { }
        public int ClientID {  get; set; }
        public int ProjectID { get; set; }

        [ForeignKey(nameof(ClientID))]
        public virtual Client Client { get; set; }

        [ForeignKey(nameof(ProjectID))]
        public virtual Project Project { get; set; }
    }
}
