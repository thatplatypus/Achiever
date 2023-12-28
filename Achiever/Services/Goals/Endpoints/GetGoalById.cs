using Achiever.Infrastucture.Endpoints;
using Achiever.Infrastucture.Extensions;
using Achiever.Services.Goals.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Achiever.Shared.Goals.Endpoints.GetGoalByIdRequestModel;

namespace Achiever.Services.Goals.Endpoints
{
    public class GetGoalById(IGoalReadRepository database) : IEndpoint<GetGoalByIdRequest, GetGoalByIdResponse>
    {
        public void Map(IEndpointRouteBuilder app) => app
            .MapGetFromQuery(this, nameof(GetGoalById))
            .WithSummary("Gets a goal by id")
            .WithDescription("Gets a goal by id");

        public async Task<EndpointResult<GetGoalByIdResponse>> Handle(GetGoalByIdRequest request, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            var entityGoal = await database.GetByIdAsync(request.Id);

            var goal = entityGoal.ToViewModel();

            return new GetGoalByIdResponse(goal);
        }
    }
}
