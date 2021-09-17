// using DarkDispatcher.Core.Domain;
// using DarkDispatcher.Core.Ids;
// using DarkDispatcher.Domain.Projects.Events;
//
// namespace DarkDispatcher.Domain.Projects
// {
//   public class Tag : Aggregate
//   {
//     private Tag()
//     {
//     }
//
//     public Tag(string organizationId, string id, string name, string color)
//     {
//       var @event = new TagCreated(organizationId, id, GetNextVersion(), name, color);
//       AddEvent(@event);
//     }
//     
//     public string Name { get; private set; }
//     public string Color { get; private set; }
//     public bool IsDeleted { get; private set; }
//
//     public void Update(string name, string color)
//     {
//       var @event = new TagUpdated(TenantId, Id, GetNextVersion(), name, color);
//       AddEvent(@event);
//     }
//
//     public void Delete()
//     {
//       var @event = new TagDeleted(TenantId, Id, GetNextVersion(), Name);
//       AddEvent(@event);
//     }
//
//     public override void When(IDomainEvent @event)
//     {
//       switch (@event)
//       {
//         case TagCreated created:
//           When(created);
//           break;
//         case TagDeleted deleted:
//           When(deleted);
//           break;
//         case TagUpdated updated:
//           When(updated);
//           break;
//       }
//     }
//
//     private void When(TagDeleted deleted)
//     {
//       IsDeleted = false;
//     }
//
//     private void When(TagCreated created)
//     {
//       TenantId = created.TenantId;
//       Id = created.Id;
//       Name = created.Name;
//       Color = created.Color;
//       IsDeleted = false;
//     }
//     
//     private void When(TagUpdated updated)
//     {
//       Name = updated.Name;
//       Color = updated.Color;
//     }
//   }
// }