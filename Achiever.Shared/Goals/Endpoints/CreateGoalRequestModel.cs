namespace Achiever.Shared.Goals.Endpoints
{
    public static class CreateGoalRequestModel
    {
        public record CreateGoalRequest(Goal Goal);

        public record CreateGoalResponse(Guid Id);
    }
}
