using MediatR;

namespace DarkDispatcher.Core.Domain
{
  public interface IQuery<out T> : IRequest<T> where T : notnull
  {
  }
}