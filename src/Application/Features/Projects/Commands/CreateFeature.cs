using System.Threading;
using System.Threading.Tasks;
using DarkDispatcher.Core.Commands;
using DarkDispatcher.Core.Ids;
using DarkDispatcher.Core.Persistence;
using DarkDispatcher.Domain.Features;
using DarkDispatcher.Domain.Projects;
using FluentValidation;

namespace DarkDispatcher.Application.Features.Projects.Commands
{
  public class CreateFeature
  {
    public record Command(ConfigurationId ConfigurationId, string Key, string Name, string? Description) : ICommand<Feature>;

    internal class Validator : AbstractValidator<Command>
    {
      public Validator()
      {
        const int min = 4;
        const int max = 50;

        RuleFor(x => x.ConfigurationId)
          .NotEmpty().WithMessage($"{nameof(ConfigurationId)} is required.");

        RuleFor(x => x.Name)
          .NotEmpty().WithMessage("Name is required.")
          .Length(min, max).WithMessage($"Name must be between {min} and {max} characters.");
        
        RuleFor(x => x.Description)
          .MaximumLength(250).WithMessage("Description must not exceed 250 characters.");
      }
    }
    
    internal class Handler : ICommandHandler<Command, Feature>
    {
      private readonly IAggregateStore _store;
      private readonly IIdGenerator _idGenerator;

      public Handler(IAggregateStore store, IIdGenerator idGenerator)
      {
        _store = store;
        _idGenerator = idGenerator;
      }
      
      public async Task<Feature> Handle(Command request, CancellationToken cancellationToken)
      {
        var id = _idGenerator.New();
        var featureId = new FeatureId(request.ConfigurationId, id);
        var feature = new Feature(featureId, request.Key, request.Name, request.Description);
        var created = await _store.StoreAsync(feature, cancellationToken);
        
        return created;
      }
    }
  }
}