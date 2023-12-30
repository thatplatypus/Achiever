using Achiever.Services.Goals.Domain;
using Achiever.Services.Goals.Entities;
using System.Net;
using static Achiever.Shared.Goals.Endpoints.GetGoalsRequestModel;

namespace Achiever.Tests.Integration.Goals
{
    public class GetGoalsTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory = factory;

        [Fact]
        public async Task GetGoals_ReturnsSeededGoals()
        {
            // Arrange
            var client = _factory.CreateClient();
            var goal1 = new GoalEntity()
            {
                Id = Guid.NewGuid(),
                Title = "Test Goal 1",
            };
            var goal2 = new GoalEntity()
            {
                Id = Guid.NewGuid(),
                Title = "Test Goal 2",
            };
            using var scope = _factory.Services.CreateScope();
            var writeRepository = scope.ServiceProvider.GetRequiredService<IGoalWriteRepository>();
            await writeRepository.AddGoalAsync(goal1);
            await writeRepository.AddGoalAsync(goal2);

            // Act
            var response = await client.GetAsync("GetGoals");

            // Assert
            response.EnsureSuccessStatusCode();
            var resp = await response.Content.ReadFromJsonAsync<GetGoalsResponse>();
            var returnedGoals = resp.Goals;
            returnedGoals.Should().NotBeNull();
            returnedGoals.Should().HaveCount(4);
            returnedGoals.First(x => x.Id == goal1.Id).Title.Should().BeEquivalentTo(goal1.Title);
            returnedGoals.First(x => x.Id == goal2.Id).Title.Should().BeEquivalentTo(goal2.Title);
        }
    }
}
