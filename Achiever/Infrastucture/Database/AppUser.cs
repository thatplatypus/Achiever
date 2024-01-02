using Microsoft.AspNetCore.Identity;

namespace Achiever.Infrastucture.Database
{
    public class AppUser : IdentityUser
    {
        public Guid AccountId { get; set; } = Guid.NewGuid();
    }
}
