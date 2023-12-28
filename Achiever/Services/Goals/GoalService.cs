using Achiever.Services.Goals.Domain;
using Achiever.Services.Goals.Entities;
using Microsoft.EntityFrameworkCore;

namespace Achiever.Services.Goals
{
    public class GoalService : IGoalReadRepository, IGoalWriteRepository
    {
        private readonly AppDbContext _context;

        public GoalService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddGoalAsync(GoalEntity goal)
        {
            _context.Goals.Add(goal);
            await _context.SaveChangesAsync();
        }

        public async Task AddSubTaskAsync(SubTaskEntity subTask)
        {
            _context.SubTasks.Add(subTask);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGoalAsync(Guid id)
        {
            var goal = await _context.Goals.FindAsync(id);
            if (goal != null)
            {
                _context.Goals.Remove(goal);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteSubTaskAsync(Guid id)
        {
            var subTask = await _context.SubTasks.FindAsync(id);
            if (subTask != null)
            {
                _context.SubTasks.Remove(subTask);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<GoalEntity>> GetAllAsync()
        {
            return await _context.Goals.ToListAsync();
        }

        public async Task<GoalEntity> GetByIdAsync(Guid id)
        {
            return await _context.Goals.FindAsync(id);
        }

        public async Task<IEnumerable<SubTaskEntity>> GetSubTasksByGoalIdAsync(Guid goalId)
        {
            return await _context.SubTasks
                .Where(st => st.GoalId == goalId)
                .ToListAsync();
        }

        public async Task UpdateGoalAsync(GoalEntity goal)
        {
            //_context.Goals.Update(goal);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSubTaskAsync(SubTaskEntity subTask)
        {
            _context.SubTasks.Update(subTask);
            await _context.SaveChangesAsync();
        }
    }
}