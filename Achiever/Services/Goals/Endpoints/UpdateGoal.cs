using Achiever.Infrastucture.Endpoints;
using Achiever.Infrastucture.Extensions;
using Achiever.Services.Goals.Domain;
using Achiever.Services.Goals.Entities;
using Achiever.Services.Goals.Models;
using Achiever.Shared.Goals;
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
            .MapPut(this)
            .WithSummary("Updates an existing goal")
            .WithDescription("Updates an existing goal");

        public async Task<EndpointResult<UpdateGoalResponse>> Handle(UpdateGoalRequest request, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            var goal = await readRepository.GetByIdAsync(request.Goal.Id ?? Guid.Empty);
            if (goal == null)
            {
                return new EndpointResult<UpdateGoalResponse>(new ValidationError("Goal not found"));
            }

            UpdateFromRequestModel(goal, request.Goal);

            await writeRepository.UpdateGoalAsync(goal);
            return new UpdateGoalResponse(goal.Id);
        }

        private static void UpdateFromRequestModel(GoalEntity goal, Goal viewModel)
        {
            goal.Title = viewModel.Title;
            goal.StartDate = viewModel.StartDate;
            goal.EndDate = viewModel.EndDate;
            goal.TargetEndDate = viewModel.TargetEndDate;
            goal.Status = (Status?)viewModel?.Status ?? Status.New;
            goal.SubTasks.ForEach(x =>
            {
                var task = viewModel.SubTasks.FirstOrDefault(request => request.Id == x.Id);
                x.Status = Enum.Parse<Status>(task.Status);
                x.Title = task.Title;
                x.EstimatedHours = task.EstimatedHours;
            });
        }
    }
}