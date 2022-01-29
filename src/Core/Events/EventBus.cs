using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace DarkDispatcher.Core.Events;

public class EventBus : IEventBus
{
  private readonly IMediator _mediator;

  public EventBus(IMediator mediator)
  {
    _mediator = mediator;
  }

  public async Task PublishAsync(DomainEvent[] events, CancellationToken cancellationToken = default)
  {
    foreach (var @event in events)
    {
      await _mediator.Publish(@event, cancellationToken);
    }
  }
}