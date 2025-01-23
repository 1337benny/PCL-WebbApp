using System.ComponentModel.DataAnnotations.Schema;

namespace PCLwebb.Models
{
    public class Checklist
    {
        public Checklist() { }
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public string CreatorID { get; set; }

        public bool IsTemplate { get; set; } = true; // Standardvärde för mallar
        public int? ParentChecklistID { get; set; } // Pekar på mallen om detta är en kopia

        [ForeignKey("ParentChecklistID")]
        public virtual Checklist? ParentChecklist { get; set; }

        [ForeignKey(nameof(CreatorID))]
        public virtual User Creator { get; set; }
        public virtual IEnumerable<Project_Has_Checklist> ProjectHasChecklists { get; set; } = new List<Project_Has_Checklist>();

        public virtual IEnumerable<ListTask> ListTasks { get; set; } = new List<ListTask>();
    }
}
