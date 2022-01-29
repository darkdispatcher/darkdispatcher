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

public class MartenRoleStore : MartenRoleStore<Role>
{
  public MartenRoleStore(IDocumentSession documentSession) : base(documentSession)
  {
  }
}

public class MartenRoleStore<TRole> :
  IQueryableRoleStore<TRole>,
  IRoleClaimStore<TRole>
  where TRole : Role
{
  private bool _disposed;
  private readonly IDocumentSession _documentSession;

  public MartenRoleStore(IDocumentSession documentSession)
  {
    _documentSession = documentSession;
  }

  public IQueryable<TRole> Roles => _documentSession.Query<TRole>();

  /// <summary>
  /// Creates a new role in a store as an asynchronous operation.
  /// </summary>
  /// <param name="role">The role to create in the store.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
  public async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken = default)
  {
    cancellationToken.ThrowIfCancellationRequested();
    ThrowIfDisposed();
    if (role == null)
      throw new ArgumentNullException(nameof(role));

    _documentSession.Insert(role);
    await _documentSession.SaveChangesAsync(cancellationToken);

    return IdentityResult.Success;
  }


  /// <summary>
  /// Updates a role in a store as an asynchronous operation.
  /// </summary>
  /// <param name="role">The role to update in the store.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
  public async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken = default)
  {
    cancellationToken.ThrowIfCancellationRequested();
    ThrowIfDisposed();
    if (role == null)
      throw new ArgumentNullException(nameof(role));

    _documentSession.Update(role);
    role.ConcurrencyStamp = Guid.NewGuid().ToString();

    try
    {
      await _documentSession.SaveChangesAsync(cancellationToken);
    }
    catch (ConcurrencyException)
    {
      return IdentityResult.Failed(new IdentityError { Description = "Concurrency error" });
    }

    return IdentityResult.Success;
  }

  /// <summary>
  /// Deletes a role from the store as an asynchronous operation.
  /// </summary>
  /// <param name="role">The role to delete from the store.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
  public async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken = default)
  {
    cancellationToken.ThrowIfCancellationRequested();

    if (role == null)
      throw new ArgumentNullException(nameof(role));

    _documentSession.Delete(role);
    await _documentSession.SaveChangesAsync(cancellationToken);

    return IdentityResult.Success;
  }

  /// <summary>
  /// Gets the ID for a role from the store as an asynchronous operation.
  /// </summary>
  /// <param name="role">The role whose ID should be returned.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>A <see cref="Task{TResult}"/> that contains the ID of the role.</returns>
  public Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken = default)
  {
    cancellationToken.ThrowIfCancellationRequested();
    ThrowIfDisposed();
    if (role == null)
      throw new ArgumentNullException(nameof(role));

    return Task.FromResult(role.Id.ToString());
  }

  /// <summary>
  /// Gets the name of a role from the store as an asynchronous operation.
  /// </summary>
  /// <param name="role">The role whose name should be returned.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>A <see cref="Task{TResult}"/> that contains the name of the role.</returns>
  public Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken = default)
  {
    cancellationToken.ThrowIfCancellationRequested();
    ThrowIfDisposed();
    if (role == null)
      throw new ArgumentNullException(nameof(role));

    return Task.FromResult(role.Name);
  }

  /// <summary>
  /// Sets the name of a role in the store as an asynchronous operation.
  /// </summary>
  /// <param name="role">The role whose name should be set.</param>
  /// <param name="roleName">The name of the role.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
  public Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken = default)
  {
    cancellationToken.ThrowIfCancellationRequested();
    ThrowIfDisposed();
    if (role == null)
      throw new ArgumentNullException(nameof(role));

    role.Name = roleName;
    return Task.CompletedTask;
  }

  /// <summary>
  /// Get a role's normalized name as an asynchronous operation.
  /// </summary>
  /// <param name="role">The role whose normalized name should be retrieved.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>A <see cref="Task{TResult}"/> that contains the name of the role.</returns>
  public Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken = default)
  {
    cancellationToken.ThrowIfCancellationRequested();
    ThrowIfDisposed();
    if (role == null)
      throw new ArgumentNullException(nameof(role));

    return Task.FromResult(role.NormalizedName);
  }

  /// <summary>
  /// Set a role's normalized name as an asynchronous operation.
  /// </summary>
  /// <param name="role">The role whose normalized name should be set.</param>
  /// <param name="normalizedName">The normalized name to set</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
  public Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken = default)
  {
    cancellationToken.ThrowIfCancellationRequested();
    ThrowIfDisposed();
    if (role == null)
      throw new ArgumentNullException(nameof(role));

    role.NormalizedName = normalizedName;
    return Task.CompletedTask;
  }

  /// <summary>
  /// Finds the role who has the specified ID as an asynchronous operation.
  /// </summary>
  /// <param name="id">The role ID to look for.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>A <see cref="Task{TResult}"/> that result of the look up.</returns>
  public Task<TRole> FindByIdAsync(string id, CancellationToken cancellationToken = default)
  {
    cancellationToken.ThrowIfCancellationRequested();
    ThrowIfDisposed();
    return Roles.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
  }

  /// <summary>
  /// Finds the role who has the specified normalized name as an asynchronous operation.
  /// </summary>
  /// <param name="normalizedName">The normalized role name to look for.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>A <see cref="Task{TResult}"/> that result of the look up.</returns>
  public Task<TRole> FindByNameAsync(string normalizedName, CancellationToken cancellationToken = default)
  {
    cancellationToken.ThrowIfCancellationRequested();
    ThrowIfDisposed();
    return Roles.FirstOrDefaultAsync(r => r.NormalizedName == normalizedName, cancellationToken);
  }

  /// <summary>
  /// Get the claims associated with the specified <paramref name="role"/> as an asynchronous operation.
  /// </summary>
  /// <param name="role">The role whose claims should be retrieved.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>A <see cref="Task{TResult}"/> that contains the claims granted to a role.</returns>
  public Task<IList<Claim>> GetClaimsAsync(TRole role, CancellationToken cancellationToken = default)
  {
    ThrowIfDisposed();
    if (role == null)
      throw new ArgumentNullException(nameof(role));

    IList<Claim> claims = role.Claims.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();
    return Task.FromResult(claims);
  }

  /// <summary>
  /// Adds the <paramref name="claim"/> given to the specified <paramref name="role"/>.
  /// </summary>
  /// <param name="role">The role to add the claim to.</param>
  /// <param name="claim">The claim to add to the role.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
  public Task AddClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default)
  {
    ThrowIfDisposed();
    if (role == null)
      throw new ArgumentNullException(nameof(role));
    if (claim == null)
      throw new ArgumentNullException(nameof(claim));

    role.Claims.Add(new RoleClaim { ClaimType = claim.Type, ClaimValue = claim.Value });
    return Task.CompletedTask;
  }

  /// <summary>
  /// Removes the <paramref name="claim"/> given from the specified <paramref name="role"/>.
  /// </summary>
  /// <param name="role">The role to remove the claim from.</param>
  /// <param name="claim">The claim to remove from the role.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
  public Task RemoveClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default)
  {
    ThrowIfDisposed();
    if (role == null)
      throw new ArgumentNullException(nameof(role));
    if (claim == null)
      throw new ArgumentNullException(nameof(claim));

    var claimsToRemove = role.Claims.Where(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value).ToList();
    foreach (var c in claimsToRemove)
    {
      role.Claims.Remove(c);
    }

    return Task.CompletedTask;
  }

  /// <summary>
  /// Throws if this class has been disposed.
  /// </summary>
  protected void ThrowIfDisposed()
  {
    if (_disposed)
    {
      throw new ObjectDisposedException(GetType().Name);
    }
  }

  /// <summary>
  /// Dispose the stores
  /// </summary>
  public void Dispose() => _disposed = true;
}
