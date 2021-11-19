using DarkDispatcher.Core.Aggregates;
using DarkDispatcher.Domain.Accounts.Events.v1;
using DarkDispatcher.Domain.Accounts.Ids;

namespace DarkDispatcher.Domain.Accounts.States;

public record OrganizationState : AggregateState<OrganizationState, OrganizationId>
{
  public OrganizationState()
  {
    On<OrganizationCreated>((state, created) => state with
    {
      Id = new OrganizationId(created.Id),
      Name = created.Name
    });
      
    On<OrganizationUpdated>((state, updated) => state with
    {
      Name = updated.Name
    });
      
    On<OrganizationDeleted>((state, deleted) => state with
    {
      IsDeleted = true
    });
  }
    
  public string Name { get; init; } = null!;
    
  public bool IsDeleted { get; init; }
}