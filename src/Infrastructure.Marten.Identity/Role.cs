using System.Collections.Generic;
using Marten.Schema;
using Microsoft.AspNetCore.Identity;

namespace DarkDispatcher.Infrastructure.Marten.Identity;

[DatabaseSchemaName(Defaults.SchemaName)]
[DocumentAlias(Defaults.RoleTable)]
public class Role : IdentityRole<string>
{
  public IList<RoleClaim> Claims { get; } = new List<RoleClaim>();
}

public class RoleClaim : IdentityRoleClaim<string>
{
}
