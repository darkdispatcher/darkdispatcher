using MediatR;

namespace DarkDispatcher.Core.Domain
{
  public interface ICommand<out TResult> : IRequest<TResult> where TResult : notnull
  {
  }
}