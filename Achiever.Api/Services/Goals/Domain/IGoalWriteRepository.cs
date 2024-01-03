using Achiever.Services.Goals.Entities;
using System.Security.Claims;

namespace Achiever.Services.Goals.Domain
{
    public interface IGoalWriteRepository
    {
        Task AddGoalAsync(GoalEntity goal);
        Task UpdateGoalAsync(GoalEntity goal);
        Task DeleteGoalAsync(Guid id);
        Task AddSubTaskAsync(SubTaskEntity subTask);
        Task UpdateSubTaskAsync(SubTaskEntity subTask);
        Task DeleteSubTaskAsync(Guid id);
    }
}
