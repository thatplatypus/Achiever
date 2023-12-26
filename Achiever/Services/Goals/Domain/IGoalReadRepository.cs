using Achiever.Services.Goals.Entities;

namespace Achiever.Services.Goals.Domain
{
    public interface IGoalReadRepository
    {
        Task<GoalEntity> GetByIdAsync(Guid id);
        Task<IEnumerable<GoalEntity>> GetAllAsync();
        Task<IEnumerable<SubTaskEntity>> GetSubTasksByGoalIdAsync(Guid goalId);
    }
}
