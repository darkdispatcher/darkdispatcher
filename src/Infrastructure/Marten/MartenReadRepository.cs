using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Core;
using DarkDispatcher.Core.Persistence;
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
    
    public async ValueTask<T> FindAsync<T>(string tenantId, string id, CancellationToken cancellationToken = default) where T : class
    {
      await using var session = _store.LightweightSession(tenantId);
      var document = await session.LoadAsync<T>(id, cancellationToken);

      return document!;
    }

    public async ValueTask<T> FindAsync<T>(string tenantId, Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default) where T : class
    {
      await using var session = _store.LightweightSession(tenantId);
      var document = await session.Query<T>().SingleOrDefaultAsync(expression, cancellationToken);

      return document;
    }

    public async ValueTask<IReadOnlyCollection<T>> ListAsync<T>(string tenantId, Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default) where T : class
    {
      await using var session = _store.LightweightSession(tenantId);
      var documents = session.Query<T>().Where(expression).ToImmutableList();

      return documents;
    }
  }
}