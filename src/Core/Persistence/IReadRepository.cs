using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DarkDispatcher.Core.Persistence
{
  public interface IReadRepository
  {
    ValueTask<T> FindAsync<T>(string tenantId, string id, CancellationToken cancellationToken = default) where T : class;
    ValueTask<T> FindAsync<T>(string tenantId, Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default) where T : class;
    ValueTask<IReadOnlyCollection<T>> ListAsync<T>(string tenantId, Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default) where T : class;
  }
}