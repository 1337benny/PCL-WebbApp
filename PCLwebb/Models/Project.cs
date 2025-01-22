namespace PCLwebb.Models
{
    public class Project
    {
        public Project() { }
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public bool IsActive { get; set; } = true;

        public virtual IEnumerable<Client_Has_Project> ClientHasProjects { get; set; } = new List<Client_Has_Project>();

        public virtual IEnumerable<Project_Has_Checklist> ProjectHasChecklists { get; set; } = new List<Project_Has_Checklist>();
    }
}
