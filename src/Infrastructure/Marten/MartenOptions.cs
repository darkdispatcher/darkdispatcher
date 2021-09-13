using Marten.Events.Daemon.Resiliency;

namespace DarkDispatcher.Infrastructure.Marten
{
  public class MartenOptions
  {
    public string ConnectionString { get; set; } = default!;
    public bool ShouldRecreateDatabase { get; set; }
    public DaemonMode DaemonMode { get; set; } = DaemonMode.Disabled;
  }
}