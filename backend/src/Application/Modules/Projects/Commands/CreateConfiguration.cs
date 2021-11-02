using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Core.Commands;
using DarkDispatcher.Core.Ids;
using DarkDispatcher.Core.Persistence;
using DarkDispatcher.Domain.Projects;
using FluentValidation;

namespace DarkDispatcher.Application.Features.Projects.Commands
{
  public class CreateConfiguration
  {
    public record Command(ProjectId ProjectId, string Name, string? Description) : ICommand<Configuration>;

    internal class Validator : AbstractValidator<Command>
    {
      public Validator(IReadRepository repository)
      {
        const int min = 4;
        const int max = 50;

        RuleFor(x => x.ProjectId)
          .NotEmpty().WithMessage($"{nameof(ProjectId)} is required.");

          // .MustAsync(async (key, cancellationToken) =>
          // {
          //   var exists = await repository.FindAsync<ConfigurationProjection>()
          // });
        
        RuleFor(x => x.Name)
          .NotEmpty().WithMessage("Name is required.")
          .Length(min, max).WithMessage($"Name must be between {min} and {max} characters.");
        
        RuleFor(x => x.Description)
          .MaximumLength(250).WithMessage("Description must not exceed 250 characters.");
      }
    }
    
    internal class Handler : ICommandHandler<Command, Configuration>
    {
      private readonly IAggregateStore _store;
      private readonly IIdGenerator _idGenerator;

      public Handler(IAggregateStore store, IIdGenerator idGenerator)
      {
        _store = store;
        _idGenerator = idGenerator;
      }
      
      public async Task<Configuration> Handle(Command request, CancellationToken cancellationToken)
      {
        var id = _idGenerator.New();
        var configurationId = new ConfigurationId(request.ProjectId, id);
        var configuration = new Configuration(configurationId, request.Name, request.Description);
        var created = await _store.StoreAsync(configuration, cancellationToken);
        
        return created;
      }
    }
  }
}