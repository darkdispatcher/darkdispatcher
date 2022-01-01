using System;
using System.Collections.Generic;
using Marten.Schema;
using Microsoft.AspNetCore.Identity;

namespace DarkDispatcher.Infrastructure.Marten.Identity;

[DatabaseSchemaName(Defaults.SchemaName)]
[DocumentAlias(Defaults.UserTable)]
public class MartenUser : IdentityUser<string>
{
  public MartenUser()
  {
    Id = Guid.NewGuid().ToString();
  }

  public MartenUser(string userName) : this()
  {
    UserName = userName;
  }
  
  public IList<string> Roles { get; set; } = new List<string>();
  public IList<MartenUserClaim> Claims { get; } = new List<MartenUserClaim>();
  public IList<MartenUserLogin> Logins { get; } = new List<MartenUserLogin>();
  public IList<MartenUserToken> Tokens { get; } = new List<MartenUserToken>();
}

public class MartenUserClaim : IdentityUserClaim<string>
{
}

public class MartenUserLogin : IdentityUserLogin<string>
{
}

public class MartenUserToken : IdentityUserToken<string>
{
}