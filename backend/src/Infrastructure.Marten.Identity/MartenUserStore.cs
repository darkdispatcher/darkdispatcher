using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Marten;
using Marten.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace DarkDispatcher.Infrastructure.Marten.Identity;

public class MartenUserStore :
  MartenUserStore<MartenUser, MartenUserClaim, MartenUserLogin, MartenUserToken>
{
  public MartenUserStore(IDocumentSession documentSession) : base(documentSession)
  {
  }
}

public class MartenUserStore<TUser, TUserClaim, TUserLogin, TUserToken> :
  UserStoreBase<TUser, Guid, TUserClaim, TUserLogin, TUserToken>,
  IProtectedUserStore<TUser>
  where TUser : MartenUser
  where TUserClaim : MartenUserClaim, new()
  where TUserLogin : MartenUserLogin, new()
  where TUserToken : MartenUserToken, new()
{
  private readonly IDocumentSession _documentSession;

  public MartenUserStore(IDocumentSession documentSession, IdentityErrorDescriber describer = null)
    : base(describer ?? new IdentityErrorDescriber())
  {
    _documentSession = documentSession;
  }

  /// <summary>
  /// A navigation property for the users the store contains.
  /// </summary>
  public override IQueryable<TUser> Users => _documentSession.Query<TUser>();

  /// <summary>
  /// Creates the specified <paramref name="user"/> in the user store.
  /// </summary>
  /// <param name="user">The user to create.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the creation operation.</returns>
  public override async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken = default)
  {
    cancellationToken.ThrowIfCancellationRequested();
    ThrowIfDisposed();
    if (user == null)
    {
      throw new ArgumentNullException(nameof(user));
    }

    _documentSession.Insert(user);
    await _documentSession.SaveChangesAsync(cancellationToken);
    return IdentityResult.Success;
  }

  /// <summary>
  /// Updates the specified <paramref name="user"/> in the user store.
  /// </summary>
  /// <param name="user">The user to update.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the update operation.</returns>
  public override async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken = default)
  {
    cancellationToken.ThrowIfCancellationRequested();
    ThrowIfDisposed();
    if (user == null)
      throw new ArgumentNullException(nameof(user));

    _documentSession.Update(user);
    user.ConcurrencyStamp = Guid.NewGuid().ToString();

    try
    {
      await _documentSession.SaveChangesAsync(cancellationToken);
    }
    catch (ConcurrencyException)
    {
      return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
    }

    return IdentityResult.Success;
  }

  /// <summary>
  /// Deletes the specified <paramref name="user"/> from the user store.
  /// </summary>
  /// <param name="user">The user to delete.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the update operation.</returns>
  public override async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken = default)
  {
    cancellationToken.ThrowIfCancellationRequested();
    ThrowIfDisposed();
    if (user == null)
      throw new ArgumentNullException(nameof(user));

    _documentSession.Delete(user);
    try
    {
      await _documentSession.SaveChangesAsync(cancellationToken);
    }
    catch (ConcurrencyException)
    {
      return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
    }

    return IdentityResult.Success;
  }

  /// <summary>
  /// Finds and returns a user, if any, who has the specified <paramref name="userId"/>.
  /// </summary>
  /// <param name="userId">The user ID to search for.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>
  /// The <see cref="Task"/> that represents the asynchronous operation, containing the user matching the specified <paramref name="userId"/> if it exists.
  /// </returns>
  public override async Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
  {
    cancellationToken.ThrowIfCancellationRequested();
    ThrowIfDisposed();
    return await Users.FirstOrDefaultAsync(user => user.Id == new Guid(userId), token: cancellationToken);
  }

  /// <summary>
  /// Finds and returns a user, if any, who has the specified normalized user name.
  /// </summary>
  /// <param name="normalizedUserName">The normalized user name to search for.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>
  /// The <see cref="Task"/> that represents the asynchronous operation, containing the user matching the specified <paramref name="normalizedUserName"/> if it exists.
  /// </returns>
  public override Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default)
  {
    cancellationToken.ThrowIfCancellationRequested();
    ThrowIfDisposed();

    return Users.FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName, cancellationToken);
  }

  /// <summary>
  /// Return a user with the matching userId if it exists.
  /// </summary>
  /// <param name="userId">The user's id.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>The user if it exists.</returns>
  protected override Task<TUser> FindUserAsync(Guid userId, CancellationToken cancellationToken)
  {
    cancellationToken.ThrowIfCancellationRequested();

    return Users.SingleOrDefaultAsync(u => u.Id.Equals(userId), cancellationToken);
  }

  /// <summary>
  /// Return a user login with the matching userId, provider, providerKey if it exists.
  /// </summary>
  /// <param name="userId">The user's id.</param>
  /// <param name="loginProvider">The login provider name.</param>
  /// <param name="providerKey">The key provided by the <paramref name="loginProvider"/> to identify a user.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>The user login if it exists.</returns>
  protected override async Task<TUserLogin> FindUserLoginAsync(Guid userId, string loginProvider, string providerKey,
    CancellationToken cancellationToken)
  {
    cancellationToken.ThrowIfCancellationRequested();

    var user = await Users.SingleOrDefaultAsync(u => u.Id.Equals(userId), cancellationToken);
    if (user == null)
      return null;

    var userLogin = user.Logins.SingleOrDefault(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey);
    return userLogin as TUserLogin;
  }

  /// <summary>
  /// Return a user login with  provider, providerKey if it exists.
  /// </summary>
  /// <param name="loginProvider">The login provider name.</param>
  /// <param name="providerKey">The key provided by the <paramref name="loginProvider"/> to identify a user.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>The user login if it exists.</returns>
  protected override async Task<TUserLogin> FindUserLoginAsync(string loginProvider, string providerKey,
    CancellationToken cancellationToken)
  {
    cancellationToken.ThrowIfCancellationRequested();

    var user = await Users.SingleOrDefaultAsync(x =>
      x.Logins.Any(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey), cancellationToken);

    if (user == null)
      return null;

    return user.Logins.SingleOrDefault(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey) as
      TUserLogin;
  }

  /// <summary>
  /// Get the claims associated with the specified <paramref name="user"/> as an asynchronous operation.
  /// </summary>
  /// <param name="user">The user whose claims should be retrieved.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>A <see cref="Task{TResult}"/> that contains the claims granted to a user.</returns>
  public override Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken = default)
  {
    ThrowIfDisposed();
    if (user == null)
      throw new ArgumentNullException(nameof(user));

    IList<Claim> claims = user.Claims.Select(u => new Claim(u.ClaimType, u.ClaimValue)).ToList();
    return Task.FromResult(claims);
  }

  /// <summary>
  /// Adds the <paramref name="claims"/> given to the specified <paramref name="user"/>.
  /// </summary>
  /// <param name="user">The user to add the claim to.</param>
  /// <param name="claims">The claim to add to the user.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
  public override Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims,
    CancellationToken cancellationToken = default)
  {
    ThrowIfDisposed();
    if (user == null)
      throw new ArgumentNullException(nameof(user));
    if (claims == null)
      throw new ArgumentNullException(nameof(claims));

    foreach (var claim in claims)
    {
      user.Claims.Add(new MartenUserClaim
      {
        ClaimType = claim.Type,
        ClaimValue = claim.Value
      });
    }

    return Task.CompletedTask;
  }

  /// <summary>
  /// Replaces the <paramref name="claim"/> on the specified <paramref name="user"/>, with the <paramref name="newClaim"/>.
  /// </summary>
  /// <param name="user">The user to replace the claim on.</param>
  /// <param name="claim">The claim replace.</param>
  /// <param name="newClaim">The new claim replacing the <paramref name="claim"/>.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
  public override Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim,
    CancellationToken cancellationToken = default)
  {
    ThrowIfDisposed();
    if (user == null)
      throw new ArgumentNullException(nameof(user));

    if (claim == null)
      throw new ArgumentNullException(nameof(claim));

    if (newClaim == null)
      throw new ArgumentNullException(nameof(newClaim));

    var claimsToReplace = user.Claims.Where(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value)
      .ToList();

    foreach (var claimToReplace in claimsToReplace)
    {
      claimToReplace.ClaimType = newClaim.Type;
      claimToReplace.ClaimValue = newClaim.Value;
    }

    return Task.CompletedTask;
  }

  /// <summary>
  /// Removes the <paramref name="claims"/> given from the specified <paramref name="user"/>.
  /// </summary>
  /// <param name="user">The user to remove the claims from.</param>
  /// <param name="claims">The claim to remove.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
  public override Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims,
    CancellationToken cancellationToken = default)
  {
    ThrowIfDisposed();
    if (user == null)
      throw new ArgumentNullException(nameof(user));
    if (claims == null)
      throw new ArgumentNullException(nameof(claims));

    foreach (var claim in claims)
    {
      var claimToRemove =
        user.Claims.FirstOrDefault(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value);
      if (claimToRemove != null)
      {
        user.Claims.Remove(claimToRemove);
      }
    }

    return Task.CompletedTask;
  }

  /// <summary>
  /// Retrieves all users with the specified claim.
  /// </summary>
  /// <param name="claim">The claim whose users should be retrieved.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>
  /// The <see cref="Task"/> contains a list of users, if any, that contain the specified claim.
  /// </returns>
  public override async Task<IList<TUser>> GetUsersForClaimAsync(Claim claim,
    CancellationToken cancellationToken = default)
  {
    cancellationToken.ThrowIfCancellationRequested();
    ThrowIfDisposed();

    if (claim == null)
      throw new ArgumentNullException(nameof(claim));

    var users = await _documentSession.Query<TUser>()
      .Where(user => user.Claims.Any(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value))
      .ToListAsync(cancellationToken);

    return users.ToList();
  }

  /// <summary>
  /// Find a user token if it exists.
  /// </summary>
  /// <param name="user">The token owner.</param>
  /// <param name="loginProvider">The login provider for the token.</param>
  /// <param name="name">The name of the token.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>The user token if it exists.</returns>
  protected override Task<TUserToken> FindTokenAsync(TUser user, string loginProvider, string name,
    CancellationToken cancellationToken)
  {
    ThrowIfDisposed();
    if (user == null)
      throw new ArgumentNullException(nameof(user));

    var token = user.Tokens.SingleOrDefault(x => x.LoginProvider == loginProvider && x.Name == name) as TUserToken;
    return Task.FromResult(token)!;
  }

  /// <summary>
  /// Add a new user token.
  /// </summary>
  /// <param name="token">The token to be added.</param>
  /// <returns></returns>
  protected override async Task AddUserTokenAsync(TUserToken token)
  {
    ThrowIfDisposed();
    if (token == null)
      throw new ArgumentNullException(nameof(token));

    var user = await Users.SingleOrDefaultAsync(x => x.Id.Equals(token.UserId));

    user?.Tokens.Add(token);
  }

  /// <summary>
  /// Remove a new user token.
  /// </summary>
  /// <param name="token">The token to be removed.</param>
  /// <returns></returns>
  protected override async Task RemoveUserTokenAsync(TUserToken token)
  {
    ThrowIfDisposed();
    if (token == null)
      throw new ArgumentNullException(nameof(token));

    var user = await Users.SingleOrDefaultAsync(x => x.Id.Equals(token.UserId));

    user?.Tokens.Remove(token);
  }

  /// <summary>
  /// Adds the <paramref name="login"/> given to the specified <paramref name="user"/>.
  /// </summary>
  /// <param name="user">The user to add the login to.</param>
  /// <param name="login">The login to add to the user.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
  public override Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken cancellationToken = default)
  {
    cancellationToken.ThrowIfCancellationRequested();
    ThrowIfDisposed();
    if (user == null)
      throw new ArgumentNullException(nameof(user));

    if (login == null)
      throw new ArgumentNullException(nameof(login));

    user.Logins.Add(new MartenUserLogin
    {
      ProviderKey = login.ProviderKey,
      LoginProvider = login.LoginProvider,
      ProviderDisplayName = login.ProviderDisplayName,
    });

    return Task.CompletedTask;
  }

  /// <summary>
  /// Removes the <paramref name="loginProvider"/> given from the specified <paramref name="user"/>.
  /// </summary>
  /// <param name="user">The user to remove the login from.</param>
  /// <param name="loginProvider">The login to remove from the user.</param>
  /// <param name="providerKey">The key provided by the <paramref name="loginProvider"/> to identify a user.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
  public override Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey,
    CancellationToken cancellationToken = default)
  {
    cancellationToken.ThrowIfCancellationRequested();
    ThrowIfDisposed();
    if (user == null)
      throw new ArgumentNullException(nameof(user));

    var login = user.Logins.SingleOrDefault(l => l.ProviderKey == providerKey &&
                                                 l.LoginProvider == loginProvider);
    if (login != null)
    {
      user.Logins.Remove(login);
    }

    return Task.CompletedTask;
  }

  /// <summary>
  /// Retrieves the associated logins for the specified <param ref="user"/>.
  /// </summary>
  /// <param name="user">The user whose associated logins to retrieve.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>
  /// The <see cref="Task"/> for the asynchronous operation, containing a list of <see cref="UserLoginInfo"/> for the specified <paramref name="user"/>, if any.
  /// </returns>
  public override Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken cancellationToken = default)
  {
    cancellationToken.ThrowIfCancellationRequested();
    ThrowIfDisposed();
    if (user == null)
      throw new ArgumentNullException(nameof(user));

    IList<UserLoginInfo> logins = user.Logins
      .Select(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey, l.ProviderDisplayName))
      .ToList();
    return Task.FromResult(logins);
  }

  /// <summary>
  /// Gets the user, if any, associated with the specified, normalized email address.
  /// </summary>
  /// <param name="normalizedEmail">The normalized email address to return the user for.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>
  /// The task object containing the results of the asynchronous lookup operation, the user if any associated with the specified normalized email address.
  /// </returns>
  public override Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
  {
    cancellationToken.ThrowIfCancellationRequested();
    ThrowIfDisposed();

    return Users.SingleOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail, cancellationToken);
  }
}