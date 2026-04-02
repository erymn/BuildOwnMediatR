using System.ComponentModel.DataAnnotations;
using System.Text;
using ZPro.Custom.CQRS.Interface;

namespace ZPro.Custom.CQRS.Handler;

public sealed class ValidatorMiddleware<TRequest, TResponse> : IHandlerMiddleware<TRequest, TResponse>
{
    private readonly IEnumerable<IRequestValidatior<TRequest>> _validators;

    public ValidatorMiddleware(IEnumerable<IRequestValidatior<TRequest>> validators)
    {
        _validators = validators;
    }
    
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        if (_validators.Any())
        {
            var failures = new List<string>();

            foreach (var validator in _validators)
            {
                var errors = await validator.ValidateAsync(request, cancellationToken);
                failures.AddRange(errors);
            }

            if (failures.Any())
            {
                // string valResult = string.Join(", ", failures);
                throw new ValidationException(failures);
            }
        }
        return await next();
    }
}

public sealed class ValidationException : Exception
{
    public ValidationException(IReadOnlyCollection<string> errors)
        : base("Request validation failed.")
    {
        Errors = errors;
    }
 
    public IReadOnlyCollection<string> Errors { get; }
}