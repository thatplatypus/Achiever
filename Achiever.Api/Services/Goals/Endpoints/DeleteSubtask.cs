using Achiever.Api.Infrastructure.Endpoints;
using Achiever.Infrastructure.Endpoints;
using Achiever.Services.Goals.Domain;
using FluentValidation;
using System.Security.Claims;
using static Achiever.Shared.Goals.Requests.DeleteSubtaskRequestModels;

namespace Achiever.Api.Services.Goals.Endpoints
{
    public class DeleteSubtask(IGoalWriteRepository repository) : IEndpoint<DeleteSubtaskRequest, DeleteSubtaskResponse>
    {
        public class DeleteSubtaskValidator : AbstractValidator<DeleteSubtaskRequest>
        {
            public DeleteSubtaskValidator()
            {
                RuleFor(x => x.SubtaskId).NotEmpty();
                RuleFor(x => x.SubtaskId).NotEqual(Guid.Empty);
            }
        }   
        public void Map(IEndpointRouteBuilder app) => app
            .MapPost(this)
            .WithSummary("Deletes an existing subtask")
            .WithDescription("Deletes an existing subtask");

        public async Task<EndpointResult<DeleteSubtaskResponse>> Handle(DeleteSubtaskRequest request, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            await repository.DeleteSubTaskAsync(request.SubtaskId, cancellationToken);

            return new DeleteSubtaskResponse(true);
        }
    }
}
