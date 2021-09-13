using System;

namespace DarkDispatcher.Core.Ids
{
  public class NullIdGenerator : IIdGenerator
  {
    public string New() => Guid.NewGuid().ToString();
  }
}