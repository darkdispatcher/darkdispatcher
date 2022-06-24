using DarkDispatcher.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace DarkDispatcher.Server.Config;

public static class JwtBearerExtensions
{
  private const string JwtSectionName = "JwtAuthentication";

  public static IDarkDispatcherBuilder ConfigureJwtBearer(this IDarkDispatcherBuilder builder)
  {
    builder.Services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, builder.Configuration.GetSection(JwtSectionName));

    builder.Services
      .AddAuthorization()
      .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer();

    return builder;
  }
}
