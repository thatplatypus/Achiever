using Achiever.Services.Goals.Domain;
using Achiever.Services.Goals.Entities;
using System.Net;
using static Achiever.Shared.Goals.Endpoints.DeleteGoalRequestModel;

namespace Achiever.Tests.Integration.Goals
{
    public class DeleteGoalTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory = factory;

        [Fact]
        public async Task DeleteGoal_Successful_Request()
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
            await writeRepository.AddGoalAsync(goal);

            // Act
            var response = await client.PostAsJsonAsync($"DeleteGoal", new DeleteGoalRequest(goal.Id));

            // Assert
            response.EnsureSuccessStatusCode();
            var deleteResponse = await response.Content.ReadFromJsonAsync<DeleteGoalResponse>();
            deleteResponse.Should().NotBeNull();
            deleteResponse.Success.Should().BeTrue();

            // Check that the goal was actually deleted
            var newScope = _factory.Services.CreateScope();
            var readRepository = newScope.ServiceProvider.GetRequiredService<IGoalReadRepository>();
            var deletedGoal = await readRepository.GetByIdAsync(goal.Id);
            deletedGoal.Should().BeNull();
        }

        [Fact]
        public async Task DeleteGoal_EmptyId_ReturnsBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync($"DeleteGoal", new DeleteGoalRequest(default));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DeleteGoal_GuidEmptyId_ReturnsBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();
            var goalId = Guid.Empty;

            // Act
            var response = await client.PostAsJsonAsync($"DeleteGoal", new DeleteGoalRequest(goalId));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
