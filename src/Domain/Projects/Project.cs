using DarkDispatcher.Core.Domain;
using DarkDispatcher.Domain.Accounts;
using DarkDispatcher.Domain.Projects.Events.v1;

namespace DarkDispatcher.Domain.Projects
{
  public record ProjectId(OrganizationId OrganizationId, string Value) 
    : AggregateWithTenantId(OrganizationId, Value);
  
  public sealed class Project : Aggregate<ProjectState, ProjectId>
  {
    private Project()
    {
    }
    
    public Project(string organizationId, string id, string name, string description) 
    {
      var @event = new ProjectCreated(organizationId, id, name, description);
      Apply(@event);
    }

    public void Update(string organizationId, string name, string description)
    {
      var @event = new ProjectUpdated(organizationId, GetId(), name, description);
      Apply(@event);
    }
    
    public void Delete(string organizationId)
    {
      var @event = new ProjectDeleted(organizationId, GetId());
      Apply(@event);
    }
  }

  public record ProjectState : AggregateState<ProjectState, ProjectId>
  {
    public string Name { get; init; }
    public string Description { get; init; }
    public bool IsDeleted { get; init; }

    public override ProjectState When(IDomainEvent @event)
    {
      return @event switch
      {
        ProjectCreated created => this with
        {
          Id = new ProjectId(new OrganizationId(created.TenantId), created.Id),
          Name = created.Name,
          Description = created.Description
        },
        ProjectUpdated updated => this with
        {
          Name = updated.Name,
          Description = updated.Description
        },
        ProjectDeleted => this with { IsDeleted = true },
        _ => this
      };
    }
  }
}