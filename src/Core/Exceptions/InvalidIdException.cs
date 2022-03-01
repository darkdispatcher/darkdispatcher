using System;
using DarkDispatcher.Core.Aggregates;

namespace DarkDispatcher.Core.Exceptions;

public class InvalidIdException : Exception
{
  public InvalidIdException(AggregateId id) : base(
    $"Aggregate id {id.GetType().Name} cannot have an empty value"
  )
  {
  }
}
