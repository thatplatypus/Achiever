namespace Achiever.Infrastructure.Endpoints
{
    using System.Reflection;
    using System.Security.Claims;
    using Achiever.Api.Infrastructure.Endpoints;
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

    public static class Extensions
    {
        public static IServiceCollection AddEndpoints(this IServiceCollection services)
        {
            var endpointTypes = Assembly.GetAssembly(typeof(Program)).GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEndpoint<,>)) && !t.IsInterface && !t.IsAbstract)
                .ToList();

            foreach (var type in endpointTypes)
            {
                services.AddScoped(typeof(IEndpoint), type);
            }

            return services;
        }
    }
}
