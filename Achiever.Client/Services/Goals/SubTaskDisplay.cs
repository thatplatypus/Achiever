namespace Achiever.Client.Services.Goals
{
    public class SubTaskDisplay
    {
        public static readonly Dictionary<string, string> Statuses = new()
        {
            { "New", "New" },
            { "InProgress", "In Progress" },
            { "Completed", "Completed" }
        };
    }
}
