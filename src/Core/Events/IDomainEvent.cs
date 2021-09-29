using MediatR;

namespace DarkDispatcher.Core.Events
{
  public interface IDomainEvent : INotification
  {
    string Id { get; init; }
  }
}