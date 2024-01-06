using Achiever.Api.Infrastructure.Endpoints;
using Achiever.Infrastructure.Database;
using Achiever.Infrastructure.Endpoints;
using Achiever.Services.Goals.Domain;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Achiever.Shared.Goals.Endpoints.GetGoalByIdRequestModel;

namespace Achiever.Services.Goals.Endpoints
{
    public class GetGoalById(IGoalReadRepository database) : IEndpoint<GetGoalByIdRequest, GetGoalByIdResponse>
    {

        public class GetGoalByIdValidator : AbstractValidator<GetGoalByIdRequest>
        {
            public GetGoalByIdValidator()
            {
                RuleFor(x => x.Id).NotEmpty();
                RuleFor(x => x.Id).NotEqual(Guid.Empty);
            }
        }   
        public void Map(IEndpointRouteBuilder app) => app
            .MapGetFromQuery(this, nameof(GetGoalById))
            .WithSummary("Gets a goal by id")
            .WithDescription("Gets a goal by id");

        public async Task<EndpointResult<GetGoalByIdResponse>> Handle(GetGoalByIdRequest request, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            var entityGoal = await database.GetByIdAsync(request.Id, cancellationToken);

            if (entityGoal == null)
                return new EndpointResult<GetGoalByIdResponse>(new ValidationError("Goal not found"));

            var goal = entityGoal.ToViewModel();

            return new GetGoalByIdResponse(goal);
        }
    }
}
