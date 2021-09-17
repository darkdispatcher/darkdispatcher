using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Core.Domain;

namespace DarkDispatcher.Core.Persistence
{
  public interface IReadRepository
  {
    ValueTask<TProjection> FindAsync<TProjection, TId>(TId id, CancellationToken cancellationToken = default)
      where TProjection : class, IProjection
      where TId : AggregateId;
    
    ValueTask<TProjection> FindAsync<TProjection, TId>(TId id, Expression<Func<TProjection, bool>> expression, CancellationToken cancellationToken = default) 
      where TProjection : class, IProjection
      where TId : AggregateId;
    
    ValueTask<IReadOnlyCollection<TProjection>> ListAsync<TProjection, TId>(TId id, Expression<Func<TProjection, bool>> expression, CancellationToken cancellationToken = default) 
      where TProjection : class, IProjection
      where TId : AggregateId;
  }
}