using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Achiever.Services.Goals.Domain;
using Achiever.Services.Goals.Entities;
using System.Net.Http.Json;
using Achiever.Shared.Goals.Endpoints;
using Achiever.Shared.Goals.ViewModels;
using static Achiever.Shared.Goals.Endpoints.UpdateGoalRequestModel;
using FluentAssertions;
using Achiever.Infrastucture.Endpoints;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace Achiever.Tests.Integration.Goals
{
    public class UpdateGoalTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory = factory;

        [Fact]
        public async Task UpdateGoal_Successful_Request()
        {
            // Arrange
            var client = _factory.CreateClient();
            var goal = new GoalEntity()
            {
                Id = Guid.NewGuid(),
                Title = "Test Goal",
            };
            using var scope = _factory.Services.CreateScope();
            var writeRepository = scope.ServiceProvider.GetRequiredService<IGoalWriteRepository>();
            await writeRepository.AddGoalAsync(goal, CancellationToken.None);

            var updatedGoal = new Goal()
            {
                Id = goal.Id,
                Title = "Updated Test Goal",
            };

            // Act
            var response = await client.PutAsJsonAsync($"UpdateGoal", new UpdateGoalRequest(updatedGoal));
            
            var newScope = _factory.Services.CreateScope();
            var readRepository = newScope.ServiceProvider.GetRequiredService<IGoalReadRepository>();
            var repoGoal = await readRepository.GetByIdAsync(goal.Id, CancellationToken.None);

            // Assert
            response.EnsureSuccessStatusCode();
            var returnedGoal = await response.Content.ReadFromJsonAsync<UpdateGoalResponse>();
            returnedGoal.Should().NotBeNull();
            returnedGoal.Id.Should().Be(updatedGoal.Id.Value);
            repoGoal.Should().NotBeNull();
            repoGoal.Title.Should().Be(updatedGoal.Title);
            repoGoal.Id.Should().Be(updatedGoal.Id.Value);
        }

        [Fact]
        public async Task UpdateGoal_EmptyTitle_ReturnsBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();
            var goal = new GoalEntity()
            {
                Id = Guid.NewGuid(),
                Title = "Test Goal",
            };
            using var scope = _factory.Services.CreateScope();
            var writeRepository = scope.ServiceProvider.GetRequiredService<IGoalWriteRepository>();
            await writeRepository.AddGoalAsync(goal, CancellationToken.None);

            var updatedGoal = new Goal()
            {
                Id = goal.Id,
                Title = "",
            };

            // Act
            var response = await client.PutAsJsonAsync($"UpdateGoal", new UpdateGoalRequest(updatedGoal));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateGoal_EmptySubTaskTitle_ReturnsBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();
            var goal = new GoalEntity()
            {
                Id = Guid.NewGuid(),
                Title = "Test Goal",
            };
            using var scope = _factory.Services.CreateScope();
            var writeRepository = scope.ServiceProvider.GetRequiredService<IGoalWriteRepository>();
            await writeRepository.AddGoalAsync(goal, CancellationToken.None);

            var updatedGoal = new Goal()
            {
                Id = goal.Id,
                Title = "Updated Goal",
                SubTasks = new List<SubTask> { new SubTask { Title = "" } }
            };

            // Act
            var response = await client.PutAsJsonAsync($"UpdateGoal", new UpdateGoalRequest(updatedGoal));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateGoal_EmptyId_ReturnsBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();
            var goal = new GoalEntity()
            {
                Id = Guid.NewGuid(),
                Title = "Test Goal",
            };
            using var scope = _factory.Services.CreateScope();
            var writeRepository = scope.ServiceProvider.GetRequiredService<IGoalWriteRepository>();
            await writeRepository.AddGoalAsync(goal, CancellationToken.None);

            var updatedGoal = new Goal()
            {
                Id = Guid.Empty,  // Empty Id
                Title = "Updated Goal",
            };

            // Act
            var response = await client.PutAsJsonAsync($"UpdateGoal", new UpdateGoalRequest(updatedGoal));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}