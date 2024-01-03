using System.ComponentModel.DataAnnotations;
using Achiever.Services.Goals.Models;

namespace Achiever.Services.Goals.Entities
{
    public class GoalEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public DateTimeOffset? TargetEndDate { get; set; }
        public List<SubTaskEntity>? SubTasks { get; set; }
        public Status? Status { get; set; }
        public DateTime? LastModified { get; set; }

        public Guid AccountId { get; set; }
    }
}
