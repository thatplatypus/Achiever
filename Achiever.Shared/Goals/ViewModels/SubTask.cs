namespace Achiever.Shared.Goals.ViewModels
{
    public class SubTask
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public DateTime LastModified { get; set; }

        public Guid GoalId { get; set; }

        public Goal? Goal { get; set; }

        public double? EstimatedHours { get; set; }

        public string? Note { get; set; }

        public int? Order { get; set; }

        public bool? UserDeleted { get; set; }
    }

    public static class SubTaskExtensions
    {
        public static SubTask Clone(this SubTask subTask)
        {
            return new SubTask
            {
                Id = subTask.Id,
                Title = subTask.Title,
                Status = subTask.Status,
                LastModified = subTask.LastModified,
                GoalId = subTask.GoalId,
                EstimatedHours = subTask.EstimatedHours,
                Note = subTask.Note,
                Order = subTask.Order
            };
        }
    }
}
