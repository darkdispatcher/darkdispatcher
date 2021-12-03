using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DarkDispatcher.Infrastructure.Identity;

public class IdentityContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
  public IdentityContext(DbContextOptions dbContextOptions)
    : base(dbContextOptions)
  {
  }
    
  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);
      
    builder.HasDefaultSchema("id");
      
    builder.Entity<IdentityUser>(b =>
    {
      b.ToTable("Users");
    });

    builder.Entity<IdentityUserClaim<string>>(b =>
    {
      b.ToTable("UserClaims");
    });

    builder.Entity<IdentityUserLogin<string>>(b =>
    {
      b.ToTable("UserLogins");
    });

    builder.Entity<IdentityUserToken<string>>(b =>
    {
      b.ToTable("UserTokens");
    });

    builder.Entity<IdentityRole>(b =>
    {
      b.ToTable("Roles");
    });

    builder.Entity<IdentityRoleClaim<string>>(b =>
    {
      b.ToTable("RoleClaims");
    });

    builder.Entity<IdentityUserRole<string>>(b =>
    {
      b.ToTable("UserRoles");
    });
  }
}