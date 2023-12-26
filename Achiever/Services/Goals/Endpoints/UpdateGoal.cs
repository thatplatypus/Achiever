using Achiever.Infrastucture.Endpoints;
using Achiever.Infrastucture.Extensions;
using Achiever.Services.Goals.Domain;
using Achiever.Services.Goals.Entities;
using Achiever.Services.Goals.Models;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using static Achiever.Shared.Goals.Endpoints.UpdateGoalRequestModel;

namespace Achiever.Services.Goals.Endpoints
{
    public class UpdateGoalRequestValidator : AbstractValidator<UpdateGoalRequest>
    {
        public UpdateGoalRequestValidator()
        {
            RuleFor(x => x.Goal.Title).NotEmpty();
            RuleFor(x => x.Goal.Id).NotEmpty();
        }
    }

    public class UpdateGoal(IGoalReadRepository readRepository, IGoalWriteRepository writeRepository) : IEndpoint<UpdateGoalRequest, UpdateGoalResponse>
    {
        public void Map(IEndpointRouteBuilder app) => app
            .MapPost<UpdateGoalRequest, UpdateGoalResponse>()
            .WithSummary("Updates an existing goal")
            .WithDescription("Updates an existing goal");

        public async Task<EndpointResult<UpdateGoalResponse>> Handle(UpdateGoalRequest request, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            var goal = await readRepository.GetByIdAsync(request.Goal.Id ?? Guid.Empty);
            if (goal == null)
            {
                return new EndpointResult<UpdateGoalResponse>(new ValidationError("Goal not found"));
            }

            goal.Title = request.Goal.Title;
            goal.StartDate = request.Goal.StartDate;
            goal.EndDate = request.Goal.EndDate;
            goal.TargetEndDate = request.Goal.TargetEndDate;
            goal.Status = (Status?)request?.Goal.Status ?? Status.New;
            goal.LastModified = DateTime.UtcNow;

            await writeRepository.UpdateGoalAsync(goal);
            return new UpdateGoalResponse(goal.Id);
        }
    }
}