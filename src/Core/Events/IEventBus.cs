using System.Threading.Tasks;

namespace DarkDispatcher.Core.Events
{
  public interface IEventBus
  {
    Task PublishAsync(params IDomainEvent[] events);
  }
}