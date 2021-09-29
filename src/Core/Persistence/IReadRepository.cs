using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Core.Aggregates;

namespace DarkDispatcher.Core.Persistence
{
  public interface IReadRepository
  {
    /// <summary>
    /// Find a projection by id
    /// </summary>
    /// <param name="id">The id of the aggregate projection</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <typeparam name="TProjection">The implementation of <see cref="IProjection"/></typeparam>
    /// <typeparam name="TId">The id of the aggregate projection.</typeparam>
    /// <returns></returns>
    ValueTask<TProjection> FindAsync<TProjection, TId>(TId id, CancellationToken cancellationToken = default)
      where TProjection : class, new()
      where TId : AggregateId;
    
    /// <summary>
    /// Find a Projection by id and expression
    /// </summary>
    /// <param name="id">The id of the aggregate projection</param>
    /// <param name="expression">The expression</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <typeparam name="TProjection">The implementation of <see cref="IProjection"/></typeparam>
    /// <typeparam name="TId">The id of the aggregate projection.</typeparam>
    /// <returns></returns>
    ValueTask<TProjection> FindAsync<TProjection, TId>(TId id, Expression<Func<TProjection, bool>> expression, CancellationToken cancellationToken = default) 
      where TProjection : class, new()
      where TId : AggregateId;
    
    /// <summary>
    /// Find all projections by id and expression
    /// </summary>
    /// <param name="id">The id of the aggregate projection</param>
    /// <param name="expression">The expression</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <typeparam name="TProjection">The implementation of <see cref="IProjection"/></typeparam>
    /// <typeparam name="TId">The id of the aggregate projection.</typeparam>
    /// <returns></returns>
    ValueTask<IReadOnlyCollection<TProjection>> ListAsync<TProjection, TId>(TId id, Expression<Func<TProjection, bool>> expression, CancellationToken cancellationToken = default) 
      where TProjection : class, new()
      where TId : AggregateId;
  }
}