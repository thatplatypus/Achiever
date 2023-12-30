namespace Achiever.Shared.Goals.Endpoints
{
    public static class DeleteGoalRequestModel
    {
        public record DeleteGoalRequest(Guid GoalId);

        public record DeleteGoalResponse(bool Success);
    }
}
