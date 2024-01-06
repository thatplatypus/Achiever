using Achiever.Infrastructure.Database;
using Achiever.Services.Goals.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<GoalEntity> Goals { get; set; }
    public DbSet<SubTaskEntity> SubTasks { get; set; }
}
