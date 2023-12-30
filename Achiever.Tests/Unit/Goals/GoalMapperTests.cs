using Xunit;
using Achiever.Services.Goals.Entities;
using Achiever.Services.Goals.Domain;
using System;
using Achiever.Services.Goals.Models;

namespace Achiever.Tests.Unit.Goals
{
    public class GoalMapperTests
    {
        [Fact]
        public void ToViewModel_Maps_GoalEntity_To_Goal()
        {
            // Arrange
            var entity = new GoalEntity
            {
                Id = Guid.NewGuid(),
                Title = "Test Goal",
                StartDate = DateTimeOffset.Now,
                EndDate = DateTimeOffset.Now.AddDays(1),
                TargetEndDate = DateTimeOffset.Now.AddDays(2),
                Status = Status.InProgress,
                LastModified = DateTime.Now
            };

            // Act
            var viewModel = entity.ToViewModel();

            // Assert
            Assert.Equal(entity.Id, viewModel.Id);
            Assert.Equal(entity.Title, viewModel.Title);
            Assert.Equal(entity.StartDate, viewModel.StartDate);
            Assert.Equal(entity.EndDate, viewModel.EndDate);
            Assert.Equal(entity.TargetEndDate, viewModel.TargetEndDate);
            Assert.Equal((int)entity.Status, viewModel.Status);
            Assert.Equal(entity.LastModified, viewModel.LastModified);
        }

        [Fact]
        public void ToViewModel_Maps_SubTaskEntity_To_SubTask()
        {
            // Arrange
            var entity = new SubTaskEntity
            {
                Id = Guid.NewGuid(),
                Title = "Test SubTask",
                GoalId = Guid.NewGuid(),
                Status = "Completed",
                LastModified = DateTime.Now
            };

            // Act
            var viewModel = entity.ToViewModel();

            // Assert
            Assert.Equal(entity.Id, viewModel.Id);
            Assert.Equal(entity.Title, viewModel.Title);
            Assert.Equal(entity.GoalId, viewModel.GoalId);
            Assert.Equal(entity.Status.ToString(), viewModel.Status);
            Assert.Equal(entity.LastModified, viewModel.LastModified);
        }
    }
}