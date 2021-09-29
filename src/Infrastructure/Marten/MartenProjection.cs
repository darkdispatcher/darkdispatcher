using System;
using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Core.Events;
using DarkDispatcher.Core.Projections;
using Marten;

namespace DarkDispatcher.Infrastructure.Marten
{
  public class MartenProjection<TEvent, TView>: IEventHandler<TEvent>
    where TView: IProjection
    where TEvent: class, IDomainEvent
  {
    private readonly IDocumentSession _session;
    private readonly Func<TEvent, Guid> _getId;

    public MartenProjection(IDocumentSession session,  Func<TEvent, Guid> getId)
    {
      _session = session ?? throw new ArgumentNullException(nameof(session));
      _getId = getId ?? throw new ArgumentNullException(nameof(getId));
    }

    public async Task Handle(TEvent @event, CancellationToken cancellationToken = default)
    {
      var entity = await _session.LoadAsync<TView>(_getId(@event), cancellationToken)
                   ?? (TView)Activator.CreateInstance(typeof(TView), true)!;

      entity.When(@event);

      _session.Store(entity);

      await _session.SaveChangesAsync(cancellationToken);
    }
  }
}