namespace Achiever.Infrastucture.Endpoints
{
    using System.Security.Claims;
    using Microsoft.AspNetCore.Mvc;
    using OneOf;

    public interface IEndpoint
    {
        void Map(IEndpointRouteBuilder app);
    }

    public interface IEndpoint<TRequest, TResponse> : IEndpoint
    {
        Task<EndpointResult<TResponse>> Handle(TRequest request, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken);
    }


    [GenerateOneOf]
    public partial class EndpointResult<TResponse> : OneOfBase<TResponse, ValidationError, Unauthorized> { }

    public record ValidationError(string Message);

    public record Unauthorized;
}
