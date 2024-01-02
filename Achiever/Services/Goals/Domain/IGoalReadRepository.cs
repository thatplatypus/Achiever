using Achiever.Services.Goals.Entities;
using System.Security.Claims;

namespace Achiever.Services.Goals.Domain
{
    public interface IGoalReadRepository
    {
        Task<GoalEntity> GetByIdAsync(Guid id);
        Task<IEnumerable<GoalEntity>> GetAllAsync();
        Task<IEnumerable<SubTaskEntity>> GetSubTasksByGoalIdAsync(Guid goalId);
        Task SetReadContext(ClaimsPrincipal claimsPrincipal);
    }
}
