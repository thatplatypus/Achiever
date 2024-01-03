using Achiever.Infrastucture.Endpoints;
using System.Reflection;

namespace Achiever.Infrastucture.Extensions
{
    public static class ServiceCollectionExtensions
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
