
namespace Achiever.Shared.Goals
{
    public class SubTask
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public DateTime LastModified { get; set; }

        public Guid GoalId { get; set; }

        public Goal Goal { get; set; }
    }
}
