namespace Achiever.Infrastucture.Endpoints.Filters
{
    public class RequestLoggingFilter<TRequest>(ILogger<RequestLoggingFilter<TRequest>> logger) : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            string currentTime = DateTime.Now.ToString("HH:mm:ss");
            logger.LogInformation("{Time} {Request}:Received", currentTime, typeof(TRequest).Name);
            return await next(context);
        }
    }
}
