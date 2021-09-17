using System;

namespace DarkDispatcher.Core.Ids
{
  public class GuidIdGenerator : IIdGenerator
  {
    public string New() => Guid.NewGuid().ToString();
  }
}