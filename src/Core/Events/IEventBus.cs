using System.Threading;
using System.Threading.Tasks;

namespace DarkDispatcher.Core.Events;

public interface IEventBus
{
  Task PublishAsync(DomainEvent[] events, CancellationToken cancellationToken = default);
}
