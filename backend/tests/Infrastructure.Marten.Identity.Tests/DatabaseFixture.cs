using System;
using System.Text.Json.Serialization;
using Marten;
using Marten.Services;
using Weasel.Core;
using Weasel.Postgresql;

namespace DarkDispatcher.Infrastructure.Marten.Identity.Tests;

public class DatabaseFixture : IDisposable
{
  public readonly IDocumentStore DocumentStore;

  public DatabaseFixture()
  {
    DocumentStore = global::Marten.DocumentStore.For(_ =>
    {
      _.Connection("Host=127.0.0.1;Port=5434;Database=DarkDispatcher;User Id=postgres;Password=DarkDispatcher20!;");
      _.AutoCreateSchemaObjects = AutoCreate.All;
      
      var systemTextJsonSerializer = new SystemTextJsonSerializer
      {
        Casing = Casing.CamelCase,
        EnumStorage = EnumStorage.AsString
      };
      systemTextJsonSerializer.Customize(o =>
      {
        o.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
      });
      _.Serializer(systemTextJsonSerializer);
      
      _.Policies.AllDocumentsAreMultiTenanted();
    });
    DocumentStore.Advanced.Clean.CompletelyRemoveAll();
    DocumentStore.Advanced.Clean.DeleteAllDocuments();
  }

  public void Dispose()
  {
    DocumentStore.Dispose();
  }
}