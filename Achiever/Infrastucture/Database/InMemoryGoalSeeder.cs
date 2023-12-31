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
                Title = "Make Goals App",
                StartDate = DateTimeOffset.UtcNow,
                EndDate = DateTimeOffset.UtcNow.AddDays(7),
                TargetEndDate = DateTimeOffset.UtcNow.AddDays(7),
                Status = Status.New,
                LastModified = DateTime.UtcNow,
                SubTasks =
                [
                    new SubTaskEntity
                    {
                        Id = Guid.NewGuid(),
                        Title = "Backend Api",
                        GoalId = Guid.NewGuid(),
                        Status = "InProgress",
                        LastModified = DateTime.UtcNow,
                        EstimatedHours = 8
                    },
                    new SubTaskEntity
                    {
                        Id = Guid.NewGuid(),
                        Title = "Frontend App",
                        GoalId = Guid.NewGuid(),
                        Status = "InProgress",
                        LastModified = DateTime.UtcNow,
                        EstimatedHours = 8
                    },
                    new SubTaskEntity
                    {
                        Id = Guid.NewGuid(),
                        Title = "Setup GitHub Repo",
                        GoalId = Guid.NewGuid(),
                        Status = "Completed",
                        LastModified = DateTime.UtcNow,
                        EstimatedHours = 1
                    },
                    new SubTaskEntity
                    {
                        Id = Guid.NewGuid(),
                        Title = "Start ReadMe",
                        GoalId = Guid.NewGuid(),
                        Status = "New",
                        LastModified = DateTime.UtcNow,
                        EstimatedHours = 0.5
                    }
                ]   
            };

            var secondGoal = new GoalEntity
            {
                Id = Guid.NewGuid(),
                Title = "Learn Elixer Basics",
                StartDate = DateTimeOffset.UtcNow,
                TargetEndDate = DateTimeOffset.UtcNow.AddDays(3),
                Status = Status.New,
                LastModified = DateTime.UtcNow,
                SubTasks =
                [
                    new SubTaskEntity
                    {
                        Id = Guid.NewGuid(),
                        Title = "Install Development Environment",
                        GoalId = Guid.NewGuid(),
                        Status = "New",
                        LastModified = DateTime.UtcNow,
                        EstimatedHours = 2.5
                    },
                    new SubTaskEntity
                    {
                        Id = Guid.NewGuid(),
                        Title = "Read Official Docs",
                        GoalId = Guid.NewGuid(),
                        Status = "New",
                        LastModified = DateTime.UtcNow,
                        EstimatedHours = 4
                    },
                    new SubTaskEntity
                    {
                        Id = Guid.NewGuid(),
                        Title = "Create Hello World project",
                        GoalId = Guid.NewGuid(),
                        Status = "New",
                        LastModified = DateTime.UtcNow,
                        EstimatedHours = 2
                    }
                ]
            };

            // Add the goal to the Goals DbSet
            context.Goals.Add(goal);
            context.Goals.Add(secondGoal);

            // Save the changes to the database
            context.SaveChanges();
        }
    }
}
