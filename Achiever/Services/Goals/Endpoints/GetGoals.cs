using Achiever.Infrastucture.Endpoints;
using Achiever.Infrastucture.Extensions;
using Achiever.Services.Goals.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Achiever.Shared.Goals.Endpoints.GetGoalsRequestModel;

namespace Achiever.Services.Goals.Endpoints
{
    public class GetGoals(IGoalReadRepository database) : IEndpoint<GetGoalsRequest, GetGoalsResponse>
    {
        public void Map(IEndpointRouteBuilder app) => app
            .MapGet(this)
            .WithSummary("Gets all goals")
            .WithDescription("Gets all goals");

        public async Task<EndpointResult<GetGoalsResponse>> Handle([FromBody] GetGoalsRequest request, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            var entityGoals = await database.GetAllAsync();

            var goals = entityGoals.Select(x => x.ToViewModel());

            return new GetGoalsResponse(goals.AsEnumerable());
        }
    }
}
