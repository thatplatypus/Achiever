using Achiever.Api.Infrastucture.Endpoints;
using Achiever.Infrastucture.Endpoints;
using Achiever.Services.Goals.Domain;
using FluentValidation;
using System.Security.Claims;
using static Achiever.Shared.Goals.Endpoints.DeleteGoalRequestModel;

namespace Achiever.Services.Goals.Endpoints
{
    public class DeleteGoalRequestValidator : AbstractValidator<DeleteGoalRequest>
    {
        public DeleteGoalRequestValidator()
        {
            RuleFor(x => x.GoalId).NotEmpty();
            RuleFor(x => x.GoalId).NotEqual(Guid.Empty);
        }
    }

    public class DeleteGoal(IGoalWriteRepository repository) : IEndpoint<DeleteGoalRequest, DeleteGoalResponse>
    {
        public void Map(IEndpointRouteBuilder app) => app
            .MapPost(this)
            .WithSummary("Deletes an existing goal")
            .WithDescription("Deletes an existing goal");

        public async Task<EndpointResult<DeleteGoalResponse>> Handle(DeleteGoalRequest request, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            await repository.DeleteGoalAsync(request.GoalId);

            return new DeleteGoalResponse(true);
        }
    }
}
