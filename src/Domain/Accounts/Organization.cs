using DarkDispatcher.Core.Domain;

namespace DarkDispatcher.Domain.Accounts
{
  public record OrganizationId(string Value) : AggregateId(Value);
  
  public sealed class Organization : Aggregate<OrganizationState, OrganizationId>
  {
    public Organization()
    {}
    
    public Organization(OrganizationId id, string name)
    {
      var @event = new Events.v1.OrganizationCreated(id, name);
      Apply(@event);
    }

    public void Update(string name)
    {
      var @event = new Events.v1.OrganizationUpdated(GetId(), name);
      Apply(@event);
    }

    public void Delete()
    {
      var @event = new Events.v1.OrganizationDeleted(GetId());
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
        Events.v1.OrganizationCreated created => this with
        {
          Id = new OrganizationId(created.Id),
          Name = created.Name
        },
        Events.v1.OrganizationUpdated updated => this with { Name = updated.Name },
        Events.v1.OrganizationDeleted => this with { IsDeleted = true },
        _ => this
      };
    }
  }
}