using System.Collections.Generic;
using DarkDispatcher.Core.Aggregates;
using DarkDispatcher.Core.Events;
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
    
    public void AddEnvironment(string organizationId, string name, string description)
    {
      var @event = new EnvironmentAdded(organizationId, GetId(), name, description);
      Apply(@event);
    }
  }

  public record ProjectState : AggregateState<ProjectState, ProjectId>
  {
    public string Name { get; init; }
    public string Description { get; init; }
    public bool IsDeleted { get; init; }

    public ICollection<Environment> Environments { get; init; }

    public override ProjectState When(IDomainEvent @event)
    {
      switch (@event)
      {
        case ProjectCreated created:
          return this with
          {
            Id = new ProjectId(new OrganizationId(created.TenantId), created.Id),
            Name = created.Name,
            Description = created.Description,
            Environments = new List<Environment>()
          };
        case ProjectUpdated updated:
          return this with { Name = updated.Name, Description = updated.Description };
        case ProjectDeleted:
          return this with { IsDeleted = true };
        case EnvironmentAdded added:
        {
          var environment = new Environment(added.EnvironmentName, added.EnvironmentDescription);
          Environments.Add(environment);
          return this;
        }
        default:
          return this;
      }
    }
  }
}