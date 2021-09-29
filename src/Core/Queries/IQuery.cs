using MediatR;

namespace DarkDispatcher.Core.Queries
{
  public interface IQuery<out T> : IRequest<T> where T : notnull
  {
  }
}