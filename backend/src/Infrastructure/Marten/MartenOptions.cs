using Marten.Events.Daemon.Resiliency;

namespace DarkDispatcher.Infrastructure.Marten
{
  public static class Defaults
  {
    public const string WriteModelSchema = "dd";
    public const string ReadModelSchema = "dd";
  }
  
  public class MartenOptions
  {
    public string ConnectionString { get; set; } = default!;
    public bool ShouldRecreateDatabase { get; set; }
    public DaemonMode DaemonMode { get; set; } = DaemonMode.Disabled;
    public string ReadModelSchema { get; set; } = Defaults.ReadModelSchema;
    public string WriteModelSchema { get; set; } = Defaults.WriteModelSchema;
  }
}