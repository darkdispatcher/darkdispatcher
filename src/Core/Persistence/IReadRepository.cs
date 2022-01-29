using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Core.Aggregates;
using DarkDispatcher.Core.Projections;

namespace DarkDispatcher.Core.Persistence;

public interface IReadRepository
{
  /// <summary>
  /// Find a projection by id
  /// </summary>
  /// <param name="id">The id of the aggregate projection</param>
  /// <param name="cancellationToken">Cancellation token</param>
  /// <typeparam name="TView">The implementation of <see cref="IView"/></typeparam>
  /// <typeparam name="TId">The id of the aggregate projection.</typeparam>
  /// <returns></returns>
  ValueTask<TView> FindAsync<TView, TId>(TId id, CancellationToken cancellationToken = default)
    where TView : class, IView
    where TId : AggregateId;

  /// <summary>
  /// Find a Projection by id and expression
  /// </summary>
  /// <param name="id">The id of the aggregate projection</param>
  /// <param name="expression">The expression</param>
  /// <param name="cancellationToken">Cancellation token</param>
  /// <typeparam name="TView">The implementation of <see cref="IView"/></typeparam>
  /// <typeparam name="TId">The id of the aggregate projection.</typeparam>
  /// <returns></returns>
  ValueTask<TView?> FindAsync<TView, TId>(TId id, Expression<Func<TView, bool>> expression, CancellationToken cancellationToken = default)
    where TView : class, IView
    where TId : AggregateId;

  /// <summary>
  /// Find all projections by id and expression
  /// </summary>
  /// <param name="id">The id of the aggregate projection</param>
  /// <param name="expression">The expression</param>
  /// <param name="cancellationToken">Cancellation token</param>
  /// <typeparam name="TView">The implementation of <see cref="IView"/></typeparam>
  /// <typeparam name="TId">The id of the aggregate projection.</typeparam>
  /// <returns></returns>
  ValueTask<IReadOnlyCollection<TView>> ListAsync<TView, TId>(TId id, Expression<Func<TView, bool>> expression, CancellationToken cancellationToken = default)
    where TView : class, IView
    where TId : AggregateId;
}
