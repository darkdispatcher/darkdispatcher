namespace DarkDispatcher.Core.Domain
{
  public interface IEntity<out TId>
  {
    /// <summary>
    /// Id for indexing our event streams
    /// </summary>
    TId Id { get; }
  }
}