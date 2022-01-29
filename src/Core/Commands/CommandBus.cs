using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace DarkDispatcher.Core.Commands;

public class CommandBus : ICommandBus
{
  private readonly IMediator _mediator;

  public CommandBus(IMediator mediator)
  {
    _mediator = mediator;
  }

  public Task<TResult> SendAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default)
    where TCommand : ICommand<TResult>
  {
    return _mediator.Send(command, cancellationToken);
  }
}
