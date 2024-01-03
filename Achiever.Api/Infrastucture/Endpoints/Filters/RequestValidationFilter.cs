using Achiever.Api.Infrastucture.Endpoints;
using FluentValidation;

namespace Achiever.Infrastucture.Endpoints.Filters
{

    public class RequestValidationFilter<TRequest>(
        ILogger<RequestValidationFilter<TRequest>> logger,
        IValidator<TRequest>? validator = null) : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var requestName = typeof(TRequest).Name;

            if (validator is null)
            {
                logger.LogDebug("[{Time}] {Request} No validator configured.", GetCurrentTime(), requestName);
                return await next(context);
            }

            logger.LogInformation("[{Time}] {Request} Validating...", GetCurrentTime(), requestName);
            var request = context.GetArgument<TRequest>(0);
            var cancellationToken = context.GetArgument<CancellationToken>(context.Arguments.Count - 1);

            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var validationError = new ValidationError(validationResult.Errors.First().ErrorMessage);
                logger.LogWarning("[{Time}] {Request} Validation failed. Reason: {ValidationError}.", GetCurrentTime(), requestName, validationError?.Message);
                return TypedResults.BadRequest(validationError);
            }

            logger.LogInformation("[{Time}] {Request} Validation succeeded.", GetCurrentTime(), requestName);
            return await next(context);
        }

        private static string GetCurrentTime()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }
    }
}
