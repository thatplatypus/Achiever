using Microsoft.AspNetCore.Identity;

namespace Achiever.Infrastructure.Database
{
    public class LocalUserSeeder
    {
        public static async Task<Guid> SeedLocalUserIfNotExists(UserManager<AppUser> userManager)
        {
            var systemUserId = await CreateUserIfNotExistsAsync(userManager, "system@localhost", "Test123!");
            var testUserId = await CreateUserIfNotExistsAsync(userManager, "test@test", "Test123!");

            return systemUserId != Guid.Empty ? systemUserId : testUserId;
        }

        private static async Task<Guid> CreateUserIfNotExistsAsync(UserManager<AppUser> userManager, string username, string password)
        {
            if (await userManager.FindByNameAsync(username) == null)
            {
                var user = new AppUser
                {
                    UserName = username,
                    Email = username,
                    EmailConfirmed = true,
                    TwoFactorEnabled = false,
                };

                var result = await userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    throw new Exception($"Failed to seed '{username}' user.");
                }

                return user.AccountId;
            }

            return Guid.Empty;
        }
    }
}