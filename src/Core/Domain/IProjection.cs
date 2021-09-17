namespace DarkDispatcher.Core.Domain
{
  public interface IProjection : IEntity<string>
  {
    void Apply(IDomainEvent @event);
  }
}