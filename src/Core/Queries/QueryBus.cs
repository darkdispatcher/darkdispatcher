using System.Threading.Tasks;
using MediatR;

namespace DarkDispatcher.Core.Queries
{
  public class QueryBus : IQueryBus
  {
    private readonly IMediator _mediator;

    public QueryBus(IMediator mediator)
    {
      _mediator = mediator;
    }

    public Task<TResponse> SendAsync<TQuery, TResponse>(TQuery query) 
      where TQuery : IQuery<TResponse>
      where TResponse : notnull
    {
      return _mediator.Send(query);
    }
  }
}