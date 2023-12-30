using Achiever.Client.Components.Goals;
using Achiever.Shared.Goals;
using Achiever.Shared.Goals.ViewModels;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Achiever.Tests.Unit.Components
{
    public class GoalCardTests : TestContext
    {
        public GoalCardTests() 
        { 
            Services.AddSingleton<IDialogService, DialogService>();

            var fluentModule = JSInterop.SetupModule("./_content/Microsoft.FluentUI.AspNetCore.Components/Components/Divider/FluentDivider.razor.js");
            fluentModule.SetupVoid("setDividerAriaOrientation");

            var apexModule = JSInterop.SetupModule("./_content/Blazor-ApexCharts/js/blazor-apexcharts.js");
            apexModule.Mode = JSRuntimeMode.Loose;
            apexModule.Setup<string>("get_apexcharts");
        }

        [Fact]
        public void GoalCard_Renders_WithNewGoal()
        {
            // Arrange
            var goal = new Goal
            {
            };

            // Act
            var cut = RenderComponent<GoalCard>(parameters => parameters
                .Add(p => p.Goal, goal));

            // Assert
            Assert.NotNull(cut);
        }
    }
}
