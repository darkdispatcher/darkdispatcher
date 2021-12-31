using System.Collections.Generic;
using Marten.Schema;
using Microsoft.AspNetCore.Identity;

namespace DarkDispatcher.Infrastructure.Marten.Identity;

[DatabaseSchemaName(Defaults.SchemaName)]
[DocumentAlias(Defaults.RoleTable)]
public class MartenRole : IdentityRole<string>
{
  public IList<MartenRoleClaim> Claims { get; } = new List<MartenRoleClaim>();
}

public class MartenRoleClaim : IdentityRoleClaim<string>
{
}