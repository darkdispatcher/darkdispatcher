using MediatR;

namespace DarkDispatcher.Core.Domain
{
  public interface IDomainEvent : INotification
  {
    string Id { get; init; }
  }
}