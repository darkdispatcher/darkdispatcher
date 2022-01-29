using System.Threading;
using System.Threading.Tasks;

namespace DarkDispatcher.Core.Commands;

public interface ICommandBus
{
  Task<TResult> SendAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default)
    where TCommand : ICommand<TResult>;
}
