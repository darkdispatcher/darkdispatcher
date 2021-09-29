using System.Threading.Tasks;

namespace DarkDispatcher.Core.Queries
{
  public interface IQueryBus
  {
    Task<TResponse> SendAsync<TQuery, TResponse>(TQuery query) 
      where TQuery : IQuery<TResponse>;
  }
}