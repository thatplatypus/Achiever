using Achiever.Services.Goals.Entities;
using Achiever.Services.Goals.Models;

namespace Achiever.Infrastucture.Database
{
    public static class InMemoryGoalSeeder
    {
        public static void SeedGoalDatabase(AppDbContext context)
        {
            // Create a new goal
            var goal = new GoalEntity
            {
                Id = Guid.NewGuid(),
                Title = "My first goal",
                StartDate = DateTimeOffset.UtcNow,
                EndDate = DateTimeOffset.UtcNow.AddDays(7),
                TargetEndDate = DateTimeOffset.UtcNow.AddDays(7),
                Status = Status.New,
                LastModified = DateTime.UtcNow,
                SubTasks = new List<SubTaskEntity>
                {
                    new SubTaskEntity
                    {
                        Id = Guid.NewGuid(),
                        Title = "My first subtask",
                        GoalId = Guid.NewGuid(),
                        Status = Status.New,
                        LastModified = DateTime.UtcNow
                    },
                    new SubTaskEntity
                    {
                        Id = Guid.NewGuid(),
                        Title = "My second subtask",
                        GoalId = Guid.NewGuid(),
                        Status = Status.New,
                        LastModified = DateTime.UtcNow
                    }
                }   
            };

            // Add the goal to the Goals DbSet
            context.Goals.Add(goal);

            // Save the changes to the database
            context.SaveChanges();
        }
    }
}
