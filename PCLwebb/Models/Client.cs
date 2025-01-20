namespace PCLwebb.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Email { get; set; }

        public virtual IEnumerable<Client_Has_Checklist> ClientHasChecklist { get; set; } = new List<Client_Has_Checklist>();
    }
}
