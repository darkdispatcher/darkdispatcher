using DarkDispatcher.Server;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args)
  .AddDarkDispatcher();

await builder.RunDarkDispatcherAsync();
