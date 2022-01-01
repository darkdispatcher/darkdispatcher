using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Marten;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Test;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DarkDispatcher.Infrastructure.Marten.Identity.Tests;

public class UserStoreTests :
  IdentitySpecificationTestBase<MartenUser, MartenRole>, IClassFixture<DatabaseFixture>,
  IDisposable
{
  private readonly DatabaseFixture _fixture;
  private IDocumentSession _session;

  public UserStoreTests(DatabaseFixture fixture)
  {
    _fixture = fixture;
  }

  protected override object CreateTestContext()
  {
    return CreateSession();
  }

  protected override void AddUserStore(IServiceCollection services, object context = null)
  {
    var session = (IDocumentSession)context;
    services.AddSingleton<IUserStore<MartenUser>>(new MartenUserStore(session));
  }

  protected override void SetUserPasswordHash(MartenUser user, string hashedPassword)
  {
    user.PasswordHash = hashedPassword;
  }

  protected override MartenUser CreateTestUser(string namePrefix = "", string email = "", string phoneNumber = "",
    bool lockoutEnabled = false, DateTimeOffset? lockoutEnd = null, bool useNamePrefixAsUserName = false)
  {
    return new MartenUser
    {
      Id = Guid.NewGuid().ToString(),
      UserName = useNamePrefixAsUserName ? namePrefix : $"{namePrefix}{Guid.NewGuid()}",
      Email = email,
      PhoneNumber = phoneNumber,
      LockoutEnabled = lockoutEnabled,
      LockoutEnd = lockoutEnd
    };
  }

  protected override Expression<Func<MartenUser, bool>> UserNameEqualsPredicate(string userName) =>
    user => user.UserName == userName;

  protected override Expression<Func<MartenUser, bool>> UserNameStartsWithPredicate(string userName) =>
    user => user.UserName.StartsWith(userName);

  private IDocumentSession CreateSession()
  {
    return _session ??= _fixture.DocumentStore.OpenSession();
  }

  protected override void AddRoleStore(IServiceCollection services, object context = null)
  {
    var session = (IDocumentSession)context;
    services.AddSingleton<IRoleStore<MartenRole>>(new MartenRoleStore(session));
  }

  protected override MartenRole CreateTestRole(string roleNamePrefix = "", bool useRoleNamePrefixAsRoleName = false)
  {
    var roleName = useRoleNamePrefixAsRoleName ? roleNamePrefix : $"{roleNamePrefix}{Guid.NewGuid()}";
    return new MartenRole
    {
      Id = Guid.NewGuid().ToString(),
      Name = roleName
    };
  }

  protected override Expression<Func<MartenRole, bool>> RoleNameEqualsPredicate(string roleName) =>
    role => role.Name == roleName;

  protected override Expression<Func<MartenRole, bool>> RoleNameStartsWithPredicate(string roleName) =>
    role => role.Name.StartsWith(roleName);

  [Fact]
  public async Task SqlUserStoreMethodsThrowWhenDisposedTest()
  {
    var store = new MartenUserStore(_session);
    store.Dispose();
    await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.AddClaimsAsync(null, null));
    await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.AddLoginAsync(null, null));
    await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.AddToRoleAsync(null, null));
    await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.GetClaimsAsync(null));
    await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.GetLoginsAsync(null));
    await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.GetRolesAsync(null));
    await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.IsInRoleAsync(null, null));
    await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.RemoveClaimsAsync(null, null));
    await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.RemoveLoginAsync(null, null, null));
    await Assert.ThrowsAsync<ObjectDisposedException>(
      async () => await store.RemoveFromRoleAsync(null, null));
    await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.RemoveClaimsAsync(null, null));
    await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.ReplaceClaimAsync(null, null, null));
    await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.FindByLoginAsync(null, null));
    await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.FindByIdAsync(null));
    await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.FindByNameAsync(null));
    await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.CreateAsync(null));
    await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.UpdateAsync(null));
    await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.DeleteAsync(null));
    await Assert.ThrowsAsync<ObjectDisposedException>(
      async () => await store.SetEmailConfirmedAsync(null, true));
    await Assert.ThrowsAsync<ObjectDisposedException>(async () => await store.GetEmailConfirmedAsync(null));
    await Assert.ThrowsAsync<ObjectDisposedException>(
      async () => await store.SetPhoneNumberConfirmedAsync(null, true));
    await Assert.ThrowsAsync<ObjectDisposedException>(
      async () => await store.GetPhoneNumberConfirmedAsync(null));
  }

  public void Dispose()
  {
    _session?.Dispose();
  }
}