using System.Threading;
using System.Threading.Tasks;

namespace DarkDispatcher.Core.Events
{
  public interface IEventBus
  {
    Task PublishAsync(IDomainEvent[] events, CancellationToken cancellationToken = default);
  }
}