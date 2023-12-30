using Achiever.Services.Goals.Entities;
using Achiever.Services.Goals.Models;
using Achiever.Shared.Goals.ViewModels;

namespace Achiever.Services.Goals.Domain
{
    public static class GoalMapper
    {
        public static Goal ToViewModel(this GoalEntity entity)
        {
            return new Goal
            {
                Id = entity.Id,
                Title = entity.Title,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                TargetEndDate = entity.TargetEndDate,
                SubTasks = entity?.SubTasks?.Select(x => x.ToViewModel()).ToList() ?? [],
                Status = (int?)entity?.Status,
                LastModified = entity?.LastModified
            };
        }

        public static SubTask ToViewModel(this SubTaskEntity entity)
        {
            return new SubTask
            {
                Id = entity.Id,
                Title = entity.Title,
                GoalId = entity.GoalId,
                Status = entity.Status.ToString(),
                LastModified = entity.LastModified,
                EstimatedHours = entity.EstimatedHours
            };
        }
        public static GoalEntity ToEntity(this Goal viewModel)
        {
            return new GoalEntity
            {
                Id = viewModel.Id ?? Guid.Empty,
                Title = viewModel.Title,
                StartDate = viewModel.StartDate,
                EndDate = viewModel.EndDate,
                TargetEndDate = viewModel.TargetEndDate,
                SubTasks = viewModel?.SubTasks?.Select(x => x.ToEntity()).ToList() ?? new List<SubTaskEntity>(),
                Status = (Status)viewModel?.Status,
                LastModified = viewModel?.LastModified
            };
        }

        public static SubTaskEntity ToEntity(this SubTask viewModel)
        {
            return new SubTaskEntity
            {
                Id = viewModel.Id,
                Title = viewModel.Title,
                GoalId = viewModel.GoalId,
                Status = viewModel.Status,
                LastModified = viewModel.LastModified,
                EstimatedHours = viewModel.EstimatedHours
            };
        }
    }
}
