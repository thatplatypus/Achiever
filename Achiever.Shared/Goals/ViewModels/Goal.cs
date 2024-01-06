namespace Achiever.Shared.Goals.ViewModels
{
    public class Goal
    {
        public Guid? Id { get; set; }
        public string? Title { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public DateTime? TargetEndDate { get; set; }
        public List<SubTask>? SubTasks { get; set; }
        public int? Status { get; set; }
        public DateTime? LastModified { get; set; }
    }

    public static class GoalExtensions
    {
        public static Goal Clone(this Goal goal)
        {
            return new Goal
            {
                Id = goal.Id,
                Title = goal.Title,
                StartDate = goal.StartDate,
                EndDate = goal.EndDate,
                TargetEndDate = goal.TargetEndDate,
                SubTasks = goal.SubTasks?.Select(x => x.Clone()).ToList(),
                Status = goal.Status,
                LastModified = goal.LastModified
            };
        }
    }
}
