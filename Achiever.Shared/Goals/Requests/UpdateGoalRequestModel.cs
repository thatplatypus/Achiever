using Achiever.Shared.Goals.ViewModels;

namespace Achiever.Shared.Goals.Endpoints
{
    public static class UpdateGoalRequestModel
    {
        public record UpdateGoalRequest(Goal Goal);

        public record UpdateGoalResponse(Guid Id);
    }
}
