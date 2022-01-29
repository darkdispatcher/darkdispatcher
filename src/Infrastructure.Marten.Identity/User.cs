using System;
using System.Collections.Generic;
using Marten.Schema;
using Microsoft.AspNetCore.Identity;

namespace DarkDispatcher.Infrastructure.Marten.Identity;

[DatabaseSchemaName(Defaults.SchemaName)]
[DocumentAlias(Defaults.UserTable)]
public class User : IdentityUser<string>
{
  public User()
  {
    Id = Guid.NewGuid().ToString();
  }

  public User(string userName) : this()
  {
    UserName = userName;
  }
  
  public IList<string> Roles { get; set; } = new List<string>();
  public IList<UserClaim> Claims { get; } = new List<UserClaim>();
  public IList<UserLogin> Logins { get; } = new List<UserLogin>();
  public IList<UserToken> Tokens { get; } = new List<UserToken>();
}

public class UserClaim : IdentityUserClaim<string>
{
}

public class UserLogin : IdentityUserLogin<string>
{
}

public class UserToken : IdentityUserToken<string>
{
}