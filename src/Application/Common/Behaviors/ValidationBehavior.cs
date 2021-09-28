using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DarkDispatcher.Application.Common.Behaviors
{
  [DebuggerStepThrough]
  public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : notnull
  {
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

    public ValidationBehavior(
      IEnumerable<IValidator<TRequest>> validators,
      ILogger<ValidationBehavior<TRequest, TResponse>> logger)
    {
      _validators = validators;
      _logger = logger;
    }

    public async Task<TResponse> Handle(
      TRequest request,
      CancellationToken cancellationToken,
      RequestHandlerDelegate<TResponse> next)
    {
      _logger.LogInformation("[{Prefix}] Handle request={X-RequestData} and response={X-ResponseData}",
        nameof(ValidationBehavior<TRequest, TResponse>), typeof(TRequest).Name, typeof(TResponse).Name);
      
      if (!_validators.Any())
        return await next();
      
      var context = new ValidationContext<TRequest>(request);
      var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
      var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();
      
      _logger.LogDebug($"Handling {typeof(TRequest).FullName} with content {JsonSerializer.Serialize(request)}");
      
      if (failures.Count != 0)
        throw new ValidationException(failures);

      var response = await next();
      
      _logger.LogInformation($"Handled {typeof(TRequest).FullName}");
      
      return response;
    }
  }
}