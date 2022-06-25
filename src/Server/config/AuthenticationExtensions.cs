using DarkDispatcher.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace DarkDispatcher.Server.Config;

public static class AuthenticationExtensions
{
  private const string JwtSectionName = "JwtAuthentication";

  /// <summary>
  /// Add JwtBearer Authentication to Server
  /// </summary>
  /// <param name="builder"></param>
  /// <returns></returns>
  public static IDarkDispatcherBuilder AddDarkDispatcherAuthentication(this IDarkDispatcherBuilder builder)
  {
    builder.Services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, builder.Configuration.GetSection(JwtSectionName));

    builder.Services
      .AddAuthorization()
      .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer();

    return builder;
  }
}
