using System.ComponentModel.DataAnnotations;
using Achiever.Services.Goals.Models;
using Achiever.Shared.Goals.ViewModels;

namespace Achiever.Services.Goals.Entities
{
    public class SubTaskEntity
    {
        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; } = "";

        public string Status { get; set; } = "";

        public DateTime LastModified { get; set; }

        public Guid GoalId { get; set; }

        public GoalEntity? Goal { get; set; }

        public double? EstimatedHours { get; set; }

        public string? Note { get; set; }

        public int? Order { get; set; }

        public SubTaskEntity() { }

        public SubTaskEntity(SubTask subTask)
        {
            Title = subTask.Title;
            Status = subTask.Status;
            LastModified = subTask.LastModified;
            GoalId = subTask.GoalId;
            EstimatedHours = subTask.EstimatedHours;
            Note = subTask.Note;
            Order = subTask.Order;
        }
    }
}
