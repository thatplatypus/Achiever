using Achiever.Api.Infrastructure.Endpoints;
using Achiever.Infrastructure.Database;
using Achiever.Infrastructure.Endpoints;
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
            RuleFor(x => x.Goal).NotNull();
            RuleFor(x => x.Goal.Title).NotEmpty().When(x => x.Goal != null);
            RuleFor(x => x.Goal.StartDate!.Value.Year).GreaterThan(1900).When(x => x.Goal != null && x.Goal.StartDate.HasValue);
            RuleFor(x => x.Goal.EndDate!.Value.Year).GreaterThan(1900).When(x => x.Goal != null && x.Goal.EndDate.HasValue);
            RuleFor(x => x.Goal.TargetEndDate!.Value.Year).GreaterThan(1900).When(x => x.Goal != null && x.Goal.TargetEndDate.HasValue);
        }
    }

    public class CreateGoal(IGoalWriteRepository repository, IAccountContext accountContext) : IEndpoint<CreateGoalRequest, CreateGoalResponse>
    {
        public void Map(IEndpointRouteBuilder app) => app
            .MapPost(this)
            .WithSummary("Creates a new goal")
            .WithDescription("Creates a new goal");

        public async Task<EndpointResult<CreateGoalResponse>> Handle(CreateGoalRequest request, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            var goal = new GoalEntity
            {
                Title = request.Goal.Title,
                StartDate = request.Goal.StartDate,
                EndDate = request.Goal.EndDate,
                TargetEndDate = request.Goal.TargetEndDate,
                Status = (Status?)request?.Goal.Status ?? Status.New,
                LastModified = DateTime.UtcNow,
                AccountId = accountContext.AccountId,
                SubTasks = request?.Goal?.SubTasks?.Select(x =>
                {
                    return new SubTaskEntity
                    {
                        Title = x.Title,
                        Status = x.Status,
                        EstimatedHours = x.EstimatedHours,
                        LastModified = DateTime.UtcNow,
                        Note = x.Note,
                        Order = x.Order
                    };
                })
                .ToList()
            };

            await repository.AddGoalAsync(goal, cancellationToken);
            return new CreateGoalResponse(goal.Id);
        }
    }
}
