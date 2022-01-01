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
    var userName = useNamePrefixAsUserName ? namePrefix : $"{namePrefix}{Guid.NewGuid()}";
    return new MartenUser(userName)
    {
      Id = Guid.NewGuid().ToString(),
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
  public async Task MartenUserStoreMethodsThrowWhenDisposedTest()
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

  [Fact]
  public async Task UserStorePublicNullCheckTest()
  {
    Assert.Throws<ArgumentNullException>("documentSession", () => new MartenUserStore(null));
    var store = new MartenUserStore(CreateSession());
    await Assert.ThrowsAsync<ArgumentNullException>("user", async () => await store.GetUserIdAsync(null));
    await Assert.ThrowsAsync<ArgumentNullException>("user", async () => await store.GetUserNameAsync(null));
    await Assert.ThrowsAsync<ArgumentNullException>("user", async () => await store.SetUserNameAsync(null, null));
    await Assert.ThrowsAsync<ArgumentNullException>("user", async () => await store.CreateAsync(null));
    await Assert.ThrowsAsync<ArgumentNullException>("user", async () => await store.UpdateAsync(null));
    await Assert.ThrowsAsync<ArgumentNullException>("user", async () => await store.DeleteAsync(null));
    await Assert.ThrowsAsync<ArgumentNullException>("user", async () => await store.AddClaimsAsync(null, null));
    await Assert.ThrowsAsync<ArgumentNullException>("user",
      async () => await store.ReplaceClaimAsync(null, null, null));
    await Assert.ThrowsAsync<ArgumentNullException>("user", async () => await store.RemoveClaimsAsync(null, null));
    await Assert.ThrowsAsync<ArgumentNullException>("user", async () => await store.GetClaimsAsync(null));
    await Assert.ThrowsAsync<ArgumentNullException>("user", async () => await store.GetLoginsAsync(null));
    await Assert.ThrowsAsync<ArgumentNullException>("user", async () => await store.GetRolesAsync(null));
    await Assert.ThrowsAsync<ArgumentNullException>("user", async () => await store.AddLoginAsync(null, null));
    await
      Assert.ThrowsAsync<ArgumentNullException>("user", async () => await store.RemoveLoginAsync(null, null, null));
    await Assert.ThrowsAsync<ArgumentNullException>("user", async () => await store.AddToRoleAsync(null, null));
    await
      Assert.ThrowsAsync<ArgumentNullException>("user",
        async () => await store.RemoveFromRoleAsync(null, null));
    await Assert.ThrowsAsync<ArgumentNullException>("user", async () => await store.IsInRoleAsync(null, null));
    await Assert.ThrowsAsync<ArgumentNullException>("user", async () => await store.GetPasswordHashAsync(null));
    await
      Assert.ThrowsAsync<ArgumentNullException>("user",
        async () => await store.SetPasswordHashAsync(null, null));
    await Assert.ThrowsAsync<ArgumentNullException>("user", async () => await store.GetSecurityStampAsync(null));
    await Assert.ThrowsAsync<ArgumentNullException>("user",
      async () => await store.SetSecurityStampAsync(null, null));
    await Assert.ThrowsAsync<ArgumentNullException>("login",
      async () => await store.AddLoginAsync(new MartenUser("fake"), null));
    await Assert.ThrowsAsync<ArgumentNullException>("claims",
      async () => await store.AddClaimsAsync(new MartenUser("fake"), null));
    await Assert.ThrowsAsync<ArgumentNullException>("claims",
      async () => await store.RemoveClaimsAsync(new MartenUser("fake"), null));
    await Assert.ThrowsAsync<ArgumentNullException>("user", async () => await store.GetEmailConfirmedAsync(null));
    await Assert.ThrowsAsync<ArgumentNullException>("user",
      async () => await store.SetEmailConfirmedAsync(null, true));
    await Assert.ThrowsAsync<ArgumentNullException>("user", async () => await store.GetEmailAsync(null));
    await Assert.ThrowsAsync<ArgumentNullException>("user", async () => await store.SetEmailAsync(null, null));
    await Assert.ThrowsAsync<ArgumentNullException>("user", async () => await store.GetPhoneNumberAsync(null));
    await Assert.ThrowsAsync<ArgumentNullException>("user", async () => await store.SetPhoneNumberAsync(null, null));
    await Assert.ThrowsAsync<ArgumentNullException>("user",
      async () => await store.GetPhoneNumberConfirmedAsync(null));
    await Assert.ThrowsAsync<ArgumentNullException>("user",
      async () => await store.SetPhoneNumberConfirmedAsync(null, true));
    await Assert.ThrowsAsync<ArgumentNullException>("user", async () => await store.GetTwoFactorEnabledAsync(null));
    await Assert.ThrowsAsync<ArgumentNullException>("user",
      async () => await store.SetTwoFactorEnabledAsync(null, true));
    await Assert.ThrowsAsync<ArgumentNullException>("user", async () => await store.GetAccessFailedCountAsync(null));
    await Assert.ThrowsAsync<ArgumentNullException>("user", async () => await store.GetLockoutEnabledAsync(null));
    await Assert.ThrowsAsync<ArgumentNullException>("user",
      async () => await store.SetLockoutEnabledAsync(null, false));
    await Assert.ThrowsAsync<ArgumentNullException>("user", async () => await store.GetLockoutEndDateAsync(null));
    await Assert.ThrowsAsync<ArgumentNullException>("user",
      async () => await store.SetLockoutEndDateAsync(null, new DateTimeOffset()));
    await Assert.ThrowsAsync<ArgumentNullException>("user", async () => await store.ResetAccessFailedCountAsync(null));
    await Assert.ThrowsAsync<ArgumentNullException>("user",
      async () => await store.IncrementAccessFailedCountAsync(null));
    await Assert.ThrowsAsync<ArgumentException>("normalizedRoleName",
      async () => await store.AddToRoleAsync(new MartenUser("fake"), null));
    await Assert.ThrowsAsync<ArgumentException>("normalizedRoleName",
      async () => await store.RemoveFromRoleAsync(new MartenUser("fake"), null));
    await Assert.ThrowsAsync<ArgumentException>("normalizedRoleName",
      async () => await store.IsInRoleAsync(new MartenUser("fake"), null));
    await Assert.ThrowsAsync<ArgumentException>("normalizedRoleName",
      async () => await store.AddToRoleAsync(new MartenUser("fake"), ""));
    await Assert.ThrowsAsync<ArgumentException>("normalizedRoleName",
      async () => await store.RemoveFromRoleAsync(new MartenUser("fake"), ""));
    await Assert.ThrowsAsync<ArgumentException>("normalizedRoleName",
      async () => await store.IsInRoleAsync(new MartenUser("fake"), ""));
  }

  [Fact]
  public async Task CanCreateUsingManager()
  {
    var manager = CreateManager();
    var guid = Guid.NewGuid().ToString();
    var user = new MartenUser { UserName = $"New{guid}" };
    IdentityResultAssert.IsSuccess(await manager.CreateAsync(user));
    IdentityResultAssert.IsSuccess(await manager.DeleteAsync(user));
  }

  [Fact]
  public async Task TwoUsersSamePasswordDifferentHash()
  {
    var manager = CreateManager();
    var userA = new MartenUser(Guid.NewGuid().ToString());
    IdentityResultAssert.IsSuccess(await manager.CreateAsync(userA, "password"));
    var userB = new MartenUser(Guid.NewGuid().ToString());
    IdentityResultAssert.IsSuccess(await manager.CreateAsync(userB, "password"));

    Assert.NotEqual(userA.PasswordHash, userB.PasswordHash);
  }

  [Fact]
  public async Task FindByEmailThrowsWithTwoUsersWithSameEmail()
  {
    var manager = CreateManager();
    var userA = new MartenUser(Guid.NewGuid().ToString())
    {
      Email = "dupe@dupe.com"
    };
    IdentityResultAssert.IsSuccess(await manager.CreateAsync(userA, "password"));
    var userB = new MartenUser(Guid.NewGuid().ToString())
    {
      Email = "dupe@dupe.com"
    };
    IdentityResultAssert.IsSuccess(await manager.CreateAsync(userB, "password"));
    await Assert.ThrowsAsync<InvalidOperationException>(async () => await manager.FindByEmailAsync("dupe@dupe.com"));
  }

  [Fact]
  public async Task AddUserToUnknownRoleFails()
  {
    var manager = CreateManager();
    var user = CreateTestUser();
    IdentityResultAssert.IsSuccess(await manager.CreateAsync(user));
    await Assert.ThrowsAsync<InvalidOperationException>(
      async () => await manager.AddToRoleAsync(user, "bogus"));
  }

  public void Dispose()
  {
    _session?.Dispose();
  }
}