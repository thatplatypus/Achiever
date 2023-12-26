using Achiever.Infrastucture.Endpoints;
using Achiever.Infrastucture.Endpoints.Filters;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Achiever.Infrastucture.Extensions
{
    public static class EndpointRouteBuilderExtensions
    {
        public static RouteHandlerBuilder MapGet<TRequest, TResponse>(this IEndpointRouteBuilder app, string? path = null)
        {
            var endpointPath = path ?? typeof(TRequest).Name.Replace("Request", string.Empty);
            return app.MapGet(endpointPath, Handle<TRequest, TResponse>)
                .AddEndpointFilter<RequestLoggingFilter<TRequest>>()
                .AddEndpointFilter<RequestValidationFilter<TRequest>>();
        }

        public static RouteHandlerBuilder MapPost<TRequest, TResponse>(this IEndpointRouteBuilder app, string? path = null)
        {
            var endpointPath = path ?? typeof(TRequest).Name.Replace("Request", string.Empty);
            return app.MapPost(endpointPath, Handle<TRequest, TResponse>)
                .AddEndpointFilter<RequestLoggingFilter<TRequest>>()
                .AddEndpointFilter<RequestValidationFilter<TRequest>>();
        }

        private static async Task<Results<Ok<TResponse>, BadRequest<ValidationError>, UnauthorizedHttpResult>> Handle<TRequest, TResponse>(TRequest request, IEndpoint<TRequest, TResponse> endpoint, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            var result = await endpoint.Handle(request, claimsPrincipal, cancellationToken);
            return result.Match<Results<Ok<TResponse>, BadRequest<ValidationError>, UnauthorizedHttpResult>>
            (
                response => TypedResults.Ok(response),
                validationError => TypedResults.BadRequest(validationError),
                _ => TypedResults.Unauthorized()
            );
        }
    }
}
