using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Core.Aggregates;
using DarkDispatcher.Core.Persistence;
using DarkDispatcher.Core.Projections;
using Marten;

namespace DarkDispatcher.Infrastructure.Marten
{
  public class MartenReadRepository : IReadRepository
  {
    private readonly IDocumentStore _store;

    public MartenReadRepository(IDocumentStore store)
    {
      _store = store;
    }

    public async ValueTask<TView> FindAsync<TView, TId>(TId id, CancellationToken cancellationToken = default)
      where TView : class, IView
      where TId : AggregateId
    {
      await using var session = GetSession(id);
      var document = await session.LoadAsync<TView>(id.Value, cancellationToken);

      return document!;
    }

    public async ValueTask<TView?> FindAsync<TView, TId>(TId id, Expression<Func<TView, bool>> expression,
      CancellationToken cancellationToken = default)
      where TView : class, IView
      where TId : AggregateId
    {
      await using var session = GetSession(id);
      var document = await session.Query<TView>().SingleOrDefaultAsync(expression, cancellationToken);

      return document;
    }

    public async ValueTask<IReadOnlyCollection<TView>> ListAsync<TView, TId>(
      TId id,
      Expression<Func<TView, bool>> expression,
      CancellationToken cancellationToken = default)
      where TView : class, IView
      where TId : AggregateId
    {
      await using var session = GetSession(id);
      var documents = session.Query<TView>().Where(expression).ToImmutableList();

      return documents;
    }

    private IQuerySession GetSession<TId>(TId id)
      where TId : AggregateId =>
      string.IsNullOrEmpty(id.TenantId) ? _store.QuerySession() : _store.QuerySession(id.TenantId);
  }
}