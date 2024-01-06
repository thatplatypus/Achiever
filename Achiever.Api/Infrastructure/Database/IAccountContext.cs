namespace Achiever.Infrastructure.Database
{
    public interface IAccountContext
    {
        public Guid AccountId { get; set; }
    }

    public class AccountContext : IAccountContext
    {
        public Guid AccountId { get; set;}
    }
}
