using Achiever.Services.Goals.Entities;
using System.Threading;

namespace Achiever.Services.Goals.Domain
{
    public interface IGoalReadRepository
    {
        Task<GoalEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<GoalEntity>> GetAllAsync(CancellationToken cancellationToken);
        Task<IEnumerable<SubTaskEntity>> GetSubTasksByGoalIdAsync(Guid goalId, CancellationToken cancellationToken);
    }
}