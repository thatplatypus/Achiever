using System.ComponentModel.DataAnnotations;
using Achiever.Services.Goals.Models;

namespace Achiever.Services.Goals.Entities
{
    public class SubTaskEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Status Status { get; set; }
        public DateTime LastModified { get; set; }

        public Guid GoalId { get; set; }

        public GoalEntity Goal { get; set; }

        public double? EstimatedHours { get; set; }
    }
}
