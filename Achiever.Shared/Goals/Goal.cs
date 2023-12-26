
namespace Achiever.Shared.Goals
{
    public class Goal
    {
        public Guid? Id { get; set; }
        public string? Title  { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public DateTimeOffset? TargetEndDate { get; set; }
        public List<SubTask>? SubTasks { get; set; }
        public int? Status { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
