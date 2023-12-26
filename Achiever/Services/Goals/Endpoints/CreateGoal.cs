using Achiever.Infrastucture.Endpoints;
using Achiever.Infrastucture.Extensions;
using Achiever.Services.Goals.Domain;
using Achiever.Services.Goals.Entities;
using Achiever.Services.Goals.Models;
using FluentValidation;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using static Achiever.Shared.Goals.Endpoints.CreateGoalRequestModel;

namespace Achiever.Services.Goals.Endpoints
{
    public class CreateGoalRequestValidator : AbstractValidator<CreateGoalRequest>
    {
        public CreateGoalRequestValidator()
        {
            RuleFor(x => x.Goal.Title).NotEmpty();
        }
    }

    public class CreateGoal(IGoalWriteRepository repository) : IEndpoint<CreateGoalRequest, CreateGoalResponse>
    {
        public void Map(IEndpointRouteBuilder app) => app
            .MapPost<CreateGoalRequest, CreateGoalResponse>()
            .WithSummary("Creates a new goal")
            .WithDescription("Creates a new goal");

        public async Task<EndpointResult<CreateGoalResponse>> Handle(CreateGoalRequest request, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            var goal = new GoalEntity
            {
                Title = request.Goal.Title,
                Id = Guid.NewGuid(),
                StartDate = request.Goal.StartDate,
                EndDate = request.Goal.EndDate,
                TargetEndDate = request.Goal.TargetEndDate,
                Status = (Status?)request?.Goal.Status ?? Status.New,
                LastModified = DateTime.UtcNow
               
            };

            await repository.AddGoalAsync(goal);
            return new CreateGoalResponse(goal.Id);
        }
    }
}
