using DarkDispatcher.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DarkDispatcher.Infrastructure.Identity;

public static class Extensions
{
  public static IDarkDispatcherBuilder AddIdentity(this IDarkDispatcherBuilder builder)
  {
    var connectionString = builder.Configuration.GetConnectionString("Identity");
    builder.Services.AddDbContext<IdentityContext>(options => options.UseNpgsql(connectionString));
    builder.Services.AddIdentityCore<IdentityUser>(options =>
      {
        options.Password.RequireDigit = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
      })
      .AddEntityFrameworkStores<IdentityContext>();
      
    return builder;
  }
}