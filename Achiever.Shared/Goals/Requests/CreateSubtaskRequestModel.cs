using Achiever.Shared.Goals.ViewModels;

namespace Achiever.Shared.Goals.Endpoints
{
    public static class CreateSubtaskRequestModel
    {
        public record CreateSubtaskRequest(Guid GoalId, SubTask Subtask);

        public record CreateSubtaskResponse(Guid SubtaskId);
    }
}
