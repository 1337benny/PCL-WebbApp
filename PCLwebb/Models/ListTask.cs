﻿using System.ComponentModel.DataAnnotations.Schema;

namespace PCLwebb.Models
{
    public class ListTask
    {
        public ListTask() { }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateOnly? EditedDate { get; set; }
        public string? Assessment {  get; set; }
        public string? Note { get; set; }

        public int? ChecklistID { get; set; }

        [ForeignKey(nameof(ChecklistID))]
        public virtual Checklist Checklist { get; set; }
    }
}
