using Achiever.Infrastucture.Database;
using Achiever.Services.Goals.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading;

namespace Achiever.Services.Goals.Domain
{
    public class GoalService : IGoalReadRepository, IGoalWriteRepository
    {
        private readonly AppDbContext _context;
        private readonly IAccountContext _account;

        public GoalService(AppDbContext context, IAccountContext account)
        {
            _context = context;
            _account = account;
        }

        public async Task AddGoalAsync(GoalEntity goal, CancellationToken cancellationToken)
        {
            _context.Goals.Add(goal);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task AddSubTaskAsync(SubTaskEntity subTask, CancellationToken cancellationToken)
        {
            _context.SubTasks.Add(subTask);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteGoalAsync(Guid id, CancellationToken cancellationToken)
        {
            var goal = await _context.Goals.FindAsync(new object[] { id }, cancellationToken);
            if (goal != null)
            {
                _context.Goals.Remove(goal);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task DeleteSubTaskAsync(Guid id, CancellationToken cancellationToken)
        {
            var subTask = await _context.SubTasks.FindAsync(new object[] { id }, cancellationToken);
            if (subTask != null)
            {
                _context.SubTasks.Remove(subTask);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<IEnumerable<GoalEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Goals
                .Where(x => x.AccountId == _account.AccountId)
                .Include(x => x.SubTasks)
                .ToListAsync(cancellationToken);
        }

        public async Task<GoalEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Goals
                .Include(x => x.SubTasks)
                .FirstOrDefaultAsync(x => x.Id == id && x.AccountId == _account.AccountId, cancellationToken);
        }

        public async Task<IEnumerable<SubTaskEntity>> GetSubTasksByGoalIdAsync(Guid goalId, CancellationToken cancellationToken)
        {
            return await _context.SubTasks
                .Where(st => st.GoalId == goalId)
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateGoalAsync(GoalEntity goal, CancellationToken cancellationToken)
        {
            //_context.Goals.Update(goal);
            //_context.SubTasks.UpdateRange(goal.SubTasks);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateSubTaskAsync(SubTaskEntity subTask, CancellationToken cancellationToken)
        {
            _context.SubTasks.Update(subTask);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}