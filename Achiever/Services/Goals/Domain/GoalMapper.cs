using Achiever.Services.Goals.Entities;
using Achiever.Shared.Goals;

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
                LastModified = entity.LastModified
            };
        }
    }
}
