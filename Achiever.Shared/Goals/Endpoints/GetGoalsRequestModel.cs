
namespace Achiever.Shared.Goals.Endpoints
{
    public static class GetGoalsRequestModel
    {
        public record GetGoalsRequest();
        public record GetGoalsResponse(IEnumerable<Goal> Goals);
    }
}
