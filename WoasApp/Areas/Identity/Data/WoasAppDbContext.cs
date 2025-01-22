using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WoasApp.Areas.Identity.Data;

namespace WoasApp.Data;

public class WoasAppDbContext : IdentityDbContext<WoasAppUser>
{

    public DbSet<UserLoginTime> UserLoginTimes { get; set; }

    public WoasAppDbContext(DbContextOptions<WoasAppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserLoginTime>()
            .HasOne<WoasAppUser>(s => s.User)
            .WithMany(g => g.LoginTimes)
            .HasForeignKey(s => s.UserId)
            .HasPrincipalKey(UserLoginTime => UserLoginTime.Id);

        builder.Entity<WoasAppUser>()
            .HasKey(u => u.Id);
    }
}
