using DarkDispatcher.Core.Domain;
using DarkDispatcher.Domain.Accounts.Events;

namespace DarkDispatcher.Domain.Accounts
{
  public record OrganizationId(string Value) : AggregateId(Value);
  
  public sealed class Organization : Aggregate<OrganizationState, OrganizationId>
  {
    public Organization()
    {}
    
    public Organization(OrganizationId id, string name)
    {
      var @event = new OrganizationCreated(id, name);
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

  public record OrganizationState : AggregateState<OrganizationState, OrganizationId>
  {
    public string Name { get; init; }
    
    public bool IsDeleted { get; init; }
    
    public override OrganizationState When(IDomainEvent @event)
    {
      return @event switch
      {
        OrganizationCreated created => this with
        {
          Id = new OrganizationId(created.OrganizationId),
          Name = created.Name
        },
        OrganizationUpdated updated => this with { Name = updated.Name },
        OrganizationDeleted => this with { IsDeleted = true },
        _ => this
      };
    }
  }
}