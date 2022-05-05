using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Core.Commands;
using DarkDispatcher.Core.Persistence;
using DarkDispatcher.Domain.Accounts;
using DarkDispatcher.Domain.Accounts.Ids;
using FluentValidation;
using MediatR;

namespace DarkDispatcher.Application.Modules.Accounts.Commands;

public class UpdateOrganization
{
  public record Command(OrganizationId OrganizationId, string Name) : ICommand<Organization>;

  internal class Validator : AbstractValidator<Command>
  {
    public Validator(IAggregateStore store)
    {
      const int min = 4;
      const int max = 50;

      RuleFor(x => x.OrganizationId).NotEmpty();

      RuleFor(x => x.Name)
        .NotEmpty().WithMessage("Name is required.")
        .Length(min, max).WithMessage($"Name must be between {min} and {max} characters.");

      RuleFor(x => x.OrganizationId)
        .MustAsync(async (id, cancellationToken) =>
        {
          var organization = await store.LoadAsync<Organization>(id, null, cancellationToken);
          var organizationExists = organization != null;
          return organizationExists;
        });
    }

    internal class Handler : ICommandHandler<Command, Organization>
    {
      private readonly IAggregateStore _store;

      public Handler(IAggregateStore store)
      {
        _store = store;
      }

      public async Task<Organization> Handle(Command request, CancellationToken cancellationToken)
      {
        var organization = await _store.LoadAsync<Organization>(request.OrganizationId, null, cancellationToken);

        organization.Update(request.Name);

        var updated = await _store.StoreAsync(organization, cancellationToken);
        return updated;
      }
    }
  }
}
