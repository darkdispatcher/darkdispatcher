// using DarkDispatcher.Core.Domain;
// using DarkDispatcher.Domain.Projects.Events;
//
// namespace DarkDispatcher.Domain.Projects
// {
//   public class Project : Aggregate
//   {
//     private Project()
//     {
//     }
//     
//     public Project(string organizationId, string id, string name, string description) 
//     {
//       var @event = new ProjectCreated(organizationId, id, name, description);
//       AddEvent(@event);
//     }
//
//     public string Name { get; private set; }
//     public string Description { get; private set; }
//     public bool IsDeleted { get; private set; }
//
//     public void Update(string name, string description)
//     {
//       var @event = new ProjectUpdated(TenantId, Id, GetNextVersion(), name, description);
//       AddEvent(@event);
//     }
//     
//     public override void When(IDomainEvent @event)
//     {
//       switch (@event)
//       {
//         case ProjectCreated created:
//           Apply(created);
//           break;
//         case ProjectUpdated updated:
//           Apply(updated);
//           break;
//       }
//     }
//
//     private void Apply(ProjectCreated created)
//     {
//       TenantId = created.TenantId;
//       Id = created.AggregateId;
//       Name = created.Name;
//       Description = created.Description;
//       IsDeleted = false;
//     }
//
//     private void Apply(ProjectUpdated updated)
//     {
//       Name = updated.Name;
//       Description = updated.Description;
//     }
//   }
// }