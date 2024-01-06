using Achiever.Services.Goals.Entities;
using System.Security.Claims;
using System.Threading;

namespace Achiever.Services.Goals.Domain
{
    public interface IGoalWriteRepository
    {
        Task AddGoalAsync(GoalEntity goal, CancellationToken cancellationToken);
        Task UpdateGoalAsync(GoalEntity goal, CancellationToken cancellationToken);
        Task DeleteGoalAsync(Guid id, CancellationToken cancellationToken);
        Task AddSubTaskAsync(SubTaskEntity subTask, CancellationToken cancellationToken);
        Task UpdateSubTaskAsync(SubTaskEntity subTask, CancellationToken cancellationToken);
        Task DeleteSubTaskAsync(Guid id, CancellationToken cancellationToken);
    }
}