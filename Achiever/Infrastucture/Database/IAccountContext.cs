using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Achiever.Infrastucture.Database
{
    public interface IAccountContext
    {
        public Guid GetAccountId(ClaimsPrincipal user);

        public Task SetContext(UserManager<AppUser> userManagere, ClaimsPrincipal user);
    }

    //Revisit this implementation when we fix the delegate scope issues with the filter
    public class AccountContext() : IAccountContext
    {
        private Dictionary<string, Guid> _accountIds = new Dictionary<string, Guid>();

        public Guid GetAccountId(ClaimsPrincipal user)
        {
            if(_accountIds.TryGetValue(user.Identity.Name, out var accountId))
            {
                return accountId;
            }

            return Guid.Empty;
        }

        public async Task SetContext(UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            var appUser = await userManager.GetUserAsync(user);
            if(_accountIds.ContainsKey(user.Identity.Name))
            {
                _accountIds[user.Identity.Name] = appUser.AccountId;
            }
            else
            {
                _accountIds.Add(user.Identity.Name, appUser.AccountId);
            }   
        }
    }
}
