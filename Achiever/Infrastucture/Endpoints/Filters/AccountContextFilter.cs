
using Achiever.Infrastucture.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Achiever.Infrastucture.Endpoints.Filters
{
    public class AccountContextFilter<TRequest>(ILogger<AccountContextFilter<TRequest>> logger, UserManager<AppUser> userManager, IAccountContext accountContext) : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var user = await userManager.GetUserAsync(context.HttpContext.User);

            if(user != null)
            {
                accountContext.AccountId = user.AccountId;

                string currentTime = DateTime.Now.ToString("HH:mm:ss");
                logger.LogInformation("[{Time}] {Request} account context set to {Id}", currentTime, typeof(TRequest).Name, accountContext.AccountId);
            }

            return await next(context);
        }
    }
}
