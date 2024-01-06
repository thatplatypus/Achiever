using Achiever.Services.Goals.Domain;
using Achiever.Services.Goals.Entities;
using Achiever.Shared.Goals.ViewModels;
using System.Net;
using System.Text.Json;
using static Achiever.Shared.Goals.Endpoints.GetGoalByIdRequestModel;

namespace Achiever.Tests.Integration.Goals
{
    public class GetGoalByIdTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory = factory;

        [Fact]
        public async Task GetGoalById_ReturnsCorrectGoal()
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

            // Act
            var response = await client.GetAsync($"GetGoalById?request={JsonSerializer.Serialize(new GetGoalByIdRequest(goal.Id))}");

            // Assert
            response.EnsureSuccessStatusCode();
            var returnedGoal = await response.Content.ReadFromJsonAsync<GetGoalByIdResponse>();
            returnedGoal?.Goal.Should().NotBeNull();
            returnedGoal.Goal.Title.Should().BeEquivalentTo(goal.Title);
        }

        [Fact]
        public async Task GetGoalById_EmptyId_ReturnsBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();           

            // Act
            var response = await client.GetAsync($"GetGoalById?request={JsonSerializer.Serialize(new GetGoalByIdRequest(default!))}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetGoalById_GuidEmptyId_ReturnsBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();
            var goalId = Guid.Empty;

            // Act
            var response = await client.GetAsync($"GetGoalById?request={JsonSerializer.Serialize(new GetGoalByIdRequest(goalId))}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
