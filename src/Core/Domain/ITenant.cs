namespace DarkDispatcher.Core.Domain
{
  public interface ITenant
  {
    /// <summary>
    /// Tenant Id
    /// </summary>
    string TenantId { get; }
  }
}