using System.ComponentModel.DataAnnotations.Schema;

namespace PCLwebb.Models
{
    public class Project_Has_Checklist
    {
        public Project_Has_Checklist() { }
        public int ProjectID { get; set; }
        public int ChecklistID { get; set; }

        [ForeignKey(nameof(ProjectID))]
        public virtual Project Project { get; set; }

        [ForeignKey(nameof(ChecklistID))]
        public virtual Checklist? Checklist { get; set; }
    }
}
