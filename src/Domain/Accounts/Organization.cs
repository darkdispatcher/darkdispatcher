using DarkDispatcher.Core.Aggregates;
using DarkDispatcher.Domain.Accounts.Events.v1;
using DarkDispatcher.Domain.Accounts.Ids;
using DarkDispatcher.Domain.Accounts.States;

namespace DarkDispatcher.Domain.Accounts;

public sealed class Organization : Aggregate<OrganizationState, OrganizationId>
{
  public Organization(OrganizationId id, string name)
  {
    var @event = new OrganizationCreated(id.Value, name);
    Apply(@event);
  }

  public void Update(string name)
  {
    var @event = new OrganizationUpdated(GetId(), name);
    Apply(@event);
  }

  public void Delete()
  {
    var @event = new OrganizationDeleted(GetId());
    Apply(@event);
  }
}
