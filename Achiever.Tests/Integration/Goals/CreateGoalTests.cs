using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Achiever.Services.Goals.Domain;
using Achiever.Services.Goals.Entities;
using System.Net.Http.Json;
using Achiever.Shared.Goals.Endpoints;
using Achiever.Shared.Goals.ViewModels;
using static Achiever.Shared.Goals.Endpoints.CreateGoalRequestModel;
using FluentAssertions;
using Achiever.Infrastructure.Endpoints;
using Achiever.Api.Infrastructure.Endpoints;

namespace Achiever.Tests.Integration.Goals
{
    public class CreateGoalTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory = factory;

        [Fact]
        public async Task CreateGoal_Successful_Request()
        {
            // Arrange
            var client = _factory.CreateClient();
            var goal = new Goal()
            {
                Title = "Test Goal",
            };

            // Act
            var response = await client.PostAsJsonAsync("CreateGoal", new CreateGoalRequest(goal));

            // Assert
            response.EnsureSuccessStatusCode();
            var id = await response.Content.ReadFromJsonAsync<CreateGoalResponse>();
            id.Should().NotBeNull();
            id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public async Task CreateGoal_RespondsWith_TitleValidationError()
        {
            // Arrange
            var client = _factory.CreateClient();
            var goal = new Goal()
            {
            };

            // Act
            var response = await client.PostAsJsonAsync("CreateGoal", new CreateGoalRequest(goal));
            var error = await response.Content.ReadFromJsonAsync<ValidationError>();

            // Assert
            response.IsSuccessStatusCode.Should().BeFalse();
            error?.Message.Should().NotBeNull();
            error?.Message.Should().Contain("Title");
        }

        [Fact]
        public async Task CreateGoal_RespondsWith_BadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();
            var invalidGoal = new
            {
                testProp = "testValue"
            };

            // Act
            var response = await client.PostAsJsonAsync("CreateGoal", invalidGoal);

            // Assert
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }
}