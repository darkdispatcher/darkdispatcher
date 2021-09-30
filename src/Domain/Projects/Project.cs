using System.Collections.Generic;
using DarkDispatcher.Core.Aggregates;
using DarkDispatcher.Domain.Accounts;
using DarkDispatcher.Domain.Projects.Events.v1;

namespace DarkDispatcher.Domain.Projects
{
  public record ProjectId(OrganizationId OrganizationId, string Value) : AggregateId(OrganizationId.Value, Value);
  
  public sealed class Project : Aggregate<ProjectState, ProjectId>
  {
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
    
    public void Delete()
    {
      var @event = new ProjectDeleted(GetTenantId()!, GetId());
      Apply(@event);
    }
    
    public void AddEnvironment(string id, string name, string description, string color)
    {
      var @event = new EnvironmentAdded(GetId(), id, name, description, color);
      Apply(@event);
    }
    
    public void UpdateEnvironment(string id, string name, string description, string color)
    {
      var @event = new EnvironmentUpdated(GetId(), id, name, description, color);
      Apply(@event);
    }
  }

  public record ProjectState : AggregateState<ProjectState, ProjectId>
  {
    public ProjectState()
    {
      On<ProjectCreated>((state, created) => state with
      {
        Id = new ProjectId(new OrganizationId(created.TenantId), created.Id),
        Name = created.Name,
        Description = created.Description
      });
      
      On<ProjectUpdated>((state, updated) => state with
      {
        Name = updated.Name,
        Description = updated.Description
      });
      
      On<ProjectDeleted>((state, _) => state with
      {
        IsDeleted = true
      });
      
      On<EnvironmentAdded>((state, added) => 
      {
        var environment = new Environment(added.Id, added.Name, added.Description, added.Color);
        state.Environments.Add(environment);
        return state;
      });
      
      On<EnvironmentUpdated>((state, updated) => 
      {
        var index = state.Environments.FindIndex(x => x.Id == updated.Id);
        if (index >= 0)
        {
          state.Environments[index] = Environments[index] with
          {
            Name = updated.Name,
            Description = updated.Description,
            Color = updated.Color
          };
        }

        return state;
      });
    }
    
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public bool IsDeleted { get; init; }
    public List<Environment> Environments { get; } = new();
  }
}