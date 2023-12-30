﻿using Achiever.Infrastucture.Endpoints;
using Achiever.Infrastucture.Extensions;
using Achiever.Services.Goals.Domain;
using Achiever.Services.Goals.Entities;
using Achiever.Services.Goals.Models;
using Achiever.Shared.Goals.ViewModels;
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
            RuleFor(x => x.Goal).NotNull();
            RuleFor(x => x.Goal.Title).NotEmpty().When(x => x.Goal != null);
            RuleFor(x => x.Goal.Id).NotEmpty().When(x => x.Goal != null);
            RuleFor(x => x.Goal.SubTasks).Custom((subTasks, context) =>
            {
                if (subTasks == null)
                {
                    return;
                }

                foreach (var subTask in subTasks)
                {
                    if (string.IsNullOrEmpty(subTask.Title))
                    {
                        context.AddFailure("Title cannot be empty");
                    }
                }
            })
            .When(x => x.Goal != null);
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
            
            //Existing SubTasks
            goal.SubTasks ??= [];
            goal.SubTasks.ForEach(x =>
            {
                var task = viewModel?.SubTasks?.FirstOrDefault(request => request.Id == x.Id);
                x.Status = task?.Status ?? "New";
                x.Title = task?.Title ?? "New Subtask";
                x.EstimatedHours = task?.EstimatedHours;
            });

            //Append new
            viewModel?.SubTasks?.Where(x => x.Id == Guid.Empty).ToList().ForEach(x =>
            {
                goal.SubTasks.Add(new SubTaskEntity
                {
                    Title = x.Title,
                    Status = x.Status,
                    EstimatedHours = x.EstimatedHours,
                    GoalId = goal.Id,
                });
            });
        }
    }
}