namespace PCLwebb.Models
{
    public class Checklist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public virtual IEnumerable<Client_Has_Checklist> ClientHasChecklist { get; set; } = new List<Client_Has_Checklist>();
    }
}
