using Achiever.Api.Infrastructure.Endpoints;
using Achiever.Infrastructure.Endpoints;
using Achiever.Services.Goals.Domain;
using Achiever.Services.Goals.Entities;
using FluentValidation;
using System.Security.Claims;
using static Achiever.Shared.Goals.Endpoints.CreateSubtaskRequestModel;

namespace Achiever.Services.Goals.Endpoints
{
    public class CreateSubtaskRequestValidator : AbstractValidator<CreateSubtaskRequest>
    {
        public CreateSubtaskRequestValidator()
        {
            RuleFor(x => x.Subtask).NotNull();
            RuleFor(x => x.GoalId).NotEqual(Guid.Empty);
        }
    }

    public class CreateSubtask(IGoalWriteRepository repository) : IEndpoint<CreateSubtaskRequest, CreateSubtaskResponse>
    {
        public void Map(IEndpointRouteBuilder app) => app
            .MapPost(this)
            .WithSummary("Creates a new subtask")
            .WithDescription("Creates a new subtask");

        public async Task<EndpointResult<CreateSubtaskResponse>> Handle(CreateSubtaskRequest request, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            var subTask = new SubTaskEntity(request.Subtask)
            {
                GoalId = request.GoalId
            };

            await repository.AddSubTaskAsync(subTask, cancellationToken);
            return new CreateSubtaskResponse(subTask.Id);
        }
    }
}
