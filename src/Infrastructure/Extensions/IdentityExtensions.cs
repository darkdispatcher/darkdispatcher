using System;
using System.Security.Claims;

namespace DarkDispatcher.Infrastructure.Extensions;

public static class IdentityExtensions
{
  public static string GetUserId(this ClaimsPrincipal principal)
  {
    try
    {
      var customerId = principal.GetClaimType<string>("sub");

      return customerId;
    }
    catch
    {
      return string.Empty;
    }
  }

  public static TType GetClaimType<TType>(this ClaimsPrincipal principal, string claimType)
  {
    var claim = principal.FindFirst(claimType);
    if (claim == null)
      throw new InvalidOperationException($"No claim of type '{claimType}' found.");

    return (TType)Convert.ChangeType(claim.Value, typeof(TType));
  }
}
