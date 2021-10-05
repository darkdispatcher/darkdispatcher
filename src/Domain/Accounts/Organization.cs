using DarkDispatcher.Core.Aggregates;

namespace DarkDispatcher.Domain.Accounts
{
  public record OrganizationId(string Value) : AggregateId(Value);

  public sealed class Organization : Aggregate<OrganizationState, OrganizationId>
  {
    public Organization(OrganizationId id, string name)
    {
      var @event = new AccountEvents.V1.OrganizationCreated(id.Value, name);
      Apply(@event);
    }

    public void Update(string name)
    {
      var @event = new AccountEvents.V1.OrganizationUpdated(GetId(), name);
      Apply(@event);
    }

    public void Delete()
    {
      var @event = new AccountEvents.V1.OrganizationDeleted(GetId());
      Apply(@event);
    }
  }

  public record OrganizationState : AggregateState<OrganizationState, OrganizationId>
  {
    public OrganizationState()
    {
      On<AccountEvents.V1.OrganizationCreated>((state, created) => state with
      {
        Id = new OrganizationId(created.Id),
        Name = created.Name
      });
      
      On<AccountEvents.V1.OrganizationUpdated>((state, updated) => state with
      {
        Name = updated.Name
      });
      
      On<AccountEvents.V1.OrganizationDeleted>((state, deleted) => state with
      {
        IsDeleted = true
      });
    }
    
    public string Name { get; init; } = null!;
    
    public bool IsDeleted { get; init; }
    
  }
}