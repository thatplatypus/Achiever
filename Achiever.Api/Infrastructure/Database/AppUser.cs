using Microsoft.AspNetCore.Identity;

namespace Achiever.Infrastructure.Database
{
    public class AppUser : IdentityUser
    {
        public Guid AccountId { get; set; } = Guid.NewGuid();
    }
}
