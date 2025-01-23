using System.ComponentModel.DataAnnotations.Schema;

namespace PCLwebb.Models
{
    public class ListTask
    {
        public ListTask() { }
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }

        public int? ChecklistID { get; set; }

        [ForeignKey(nameof(ChecklistID))]
        public virtual Checklist Checklist { get; set; }
    }
}
