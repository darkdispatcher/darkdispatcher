using MediatR;

namespace DarkDispatcher.Core.Commands
{
  public interface ICommandHandler<in TCommand, TResult>: IRequestHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
  {
  }
}