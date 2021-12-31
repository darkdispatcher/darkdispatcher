using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace DarkDispatcher.Infrastructure.Marten.Identity;

public class MartenRole : IdentityRole<Guid>
{
  public IList<MartenRoleClaim> Claims { get; } = new List<MartenRoleClaim>();
}

public class MartenRoleClaim : IdentityRoleClaim<Guid>
{
}