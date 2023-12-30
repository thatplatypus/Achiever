using Achiever.Client.Components.Goals;
using Achiever.Shared.Goals;
using Achiever.Shared.Goals.ViewModels;
using Bunit;

namespace Achiever.Tests.Unit.Components
{
    public class GoalCardTests : TestContext
    {
        [Fact]
        public void GoalCard_Renders_WithNewGoal()
        {
            // Arrange
            var ctx = new TestContext();
            var goal = new Goal
            {
            };

            // Act
            var cut = ctx.RenderComponent<GoalCard>(parameters => parameters
                .Add(p => p.Goal, goal));

            // Assert
            Assert.NotNull(cut);
        }
    }
}
