using MediatR;

namespace DarkDispatcher.Core.Commands;

public interface ICommand<out TResult> : IRequest<TResult>
{
}