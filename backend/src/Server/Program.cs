using DarkDispatcher.Server;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

await builder
  .AddDarkDispatcher()
  .RunDarkDispatcherAsync();
