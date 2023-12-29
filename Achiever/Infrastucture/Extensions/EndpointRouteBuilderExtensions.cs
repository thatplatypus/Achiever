using Achiever.Infrastucture.Endpoints;
using Achiever.Infrastucture.Endpoints.Filters;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Achiever.Infrastucture.Extensions
{
    public static class EndpointRouteBuilderExtensions
    {
        /// <summary>
        /// Maps a parameterless Get request
        /// </summary>
        public static RouteHandlerBuilder MapGet<TRequest, TResponse>(this IEndpointRouteBuilder app, IEndpoint<TRequest, TResponse> endpoint, string? path = null)
        {
            var endpointPath = GetEndpointPath<TRequest>(path);
            return app.MapGet(endpointPath, ([FromBody] TRequest request, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken) => Handle(request, endpoint, claimsPrincipal, cancellationToken))
                .AddEndpointFilterPipeline<TRequest>();
        }

        /// <summary>
        /// Maps a parametered Get request, alllows passing parameters via query string
        /// </summary>
        public static RouteHandlerBuilder MapGetFromQuery<TRequest, TResponse>(this IEndpointRouteBuilder app, IEndpoint<TRequest, TResponse> endpoint, string? path = null)
        {
            var endpointPath = GetEndpointPath<TRequest>(path); 
            return app.MapGet(endpointPath, ([FromQuery] TRequest request, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken) => Handle(request, endpoint, claimsPrincipal, cancellationToken))
                .AddEndpointFilterPipeline<TRequest>();
        }

        /// <summary>
        /// Maps a Post request
        /// </summary>
        public static RouteHandlerBuilder MapPost<TRequest, TResponse>(this IEndpointRouteBuilder app, IEndpoint<TRequest, TResponse> endpoint, string? path = null)
        {
            var endpointPath = GetEndpointPath<TRequest>(path);
            return app.MapPost(endpointPath, (TRequest request, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken) => Handle(request, endpoint, claimsPrincipal, cancellationToken))
                .AddEndpointFilterPipeline<TRequest>();
        }

        /// <summary>
        /// Maps a Put request
        /// </summary>
        public static RouteHandlerBuilder MapPut<TRequest, TResponse>(this IEndpointRouteBuilder app, IEndpoint<TRequest, TResponse> endpoint, string? path = null)
        {
            var endpointPath = GetEndpointPath<TRequest>(path);
            return app.MapPut(endpointPath, (TRequest request, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken) => Handle(request, endpoint, claimsPrincipal, cancellationToken))
                .AddEndpointFilterPipeline<TRequest>();
        }

        public static async Task<Results<Ok<TResponse>, BadRequest<ValidationError>, NotFound<ValidationError>, UnauthorizedHttpResult>> Handle<TRequest, TResponse>([FromBody] TRequest request, IEndpoint<TRequest, TResponse> endpoint, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            var result = await endpoint.Handle(request, claimsPrincipal, cancellationToken);
            return result.Match<Results<Ok<TResponse>, BadRequest<ValidationError>, NotFound<ValidationError>, UnauthorizedHttpResult>>
            (
                response => TypedResults.Ok(response),
                validationError => validationError.Message.Contains("not found", StringComparison.OrdinalIgnoreCase) 
                    ? TypedResults.NotFound(validationError)
                    : TypedResults.BadRequest(validationError),
                _ => TypedResults.Unauthorized()
            );
        }

        private static string GetEndpointPath<TRequest>(string? path)
        {
            return path ?? typeof(TRequest).Name.Replace("Request", string.Empty);
        }

        private static RouteHandlerBuilder AddEndpointFilterPipeline<TRequest>(this RouteHandlerBuilder builder)
        {
            return builder
                .AddEndpointFilter<RequestLoggingFilter<TRequest>>()
                .AddEndpointFilter<RequestValidationFilter<TRequest>>();
        }
    }
}
