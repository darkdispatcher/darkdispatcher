using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace DarkDispatcher.Infrastructure.Marten.Identity;

public class MartenUser : IdentityUser<Guid>
{
  public IList<string> Roles { get; set; } = new List<string>();
  public IList<MartenUserClaim> Claims { get; } = new List<MartenUserClaim>();
  public IList<MartenUserLogin> Logins { get; } = new List<MartenUserLogin>();
  public IList<MartenUserToken> Tokens { get; } = new List<MartenUserToken>();
}

public class MartenUserClaim : IdentityUserClaim<Guid>
{
}

public class MartenUserLogin : IdentityUserLogin<Guid>
{
}

public class MartenUserToken : IdentityUserToken<Guid>
{
}