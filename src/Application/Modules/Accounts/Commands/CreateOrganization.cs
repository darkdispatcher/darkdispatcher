using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Core.Commands;
using DarkDispatcher.Core.Ids;
using DarkDispatcher.Core.Persistence;
using DarkDispatcher.Domain.Accounts;
using DarkDispatcher.Domain.Accounts.Ids;
using FluentValidation;

namespace DarkDispatcher.Application.Modules.Accounts.Commands;

public class CreateOrganization
{
  public record Command(string Name) : ICommand<Organization>;

  internal class Validator : AbstractValidator<Command>
  {
    public Validator()
    {
      const int min = 4;
      const int max = 50;

      RuleFor(x => x.Name)
        .NotEmpty().WithMessage("Name is required.")
        .Length(min, max).WithMessage($"Name must be between {min} and {max} characters.");
    }
  }

  internal class Handler : ICommandHandler<Command, Organization>
  {
    private readonly IAggregateStore _store;
    private readonly IIdGenerator _idGenerator;

    public Handler(IAggregateStore store, IIdGenerator idGenerator)
    {
      _store = store;
      _idGenerator = idGenerator;
    }

    public async Task<Organization> Handle(Command request, CancellationToken cancellationToken)
    {
      var id = _idGenerator.New();
      var organization = new Organization(new OrganizationId(id), request.Name);
      var created = await _store.StoreAsync(organization, cancellationToken);

      return created;
    }
  }
}
