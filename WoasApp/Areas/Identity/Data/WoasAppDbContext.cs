using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
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
            .HasOne(ult => ult.User)
            .WithMany(u => u.LoginTimes)
            .HasForeignKey(ult => ult.UserId);
    }
}
