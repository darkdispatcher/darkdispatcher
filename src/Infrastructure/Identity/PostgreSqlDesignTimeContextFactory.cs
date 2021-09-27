using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DarkDispatcher.Infrastructure.Identity
{
  internal class PostgreSqlDesignTimeContextFactory : IDesignTimeDbContextFactory<IdentityContext>
  {
    public IdentityContext CreateDbContext(string[] args)
    {
      var optionsBuilder = new DbContextOptionsBuilder<IdentityContext>();
      var connectionString = args.Any() ? args[0] : "Host=127.0.0.1;Port=5434;Database=DarkDispatcher;User Id=postgres;Password=DarkDispatcher20!;";
      optionsBuilder.UseNpgsql(connectionString);

      return new IdentityContext(optionsBuilder.Options);
    }
  }
}