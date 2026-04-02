using ZPro.Custom.CQRS.Interface;

namespace CustomImplementation.CommandHandler;

public sealed class CreateOrderValidator : IRequestValidatior<CreateOrderCommand>
{
    public async Task<IReadOnlyCollection<string>> ValidateAsync(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var errors = new List<string>();
        if (request.CustomerId == Guid.Empty)
        {
            errors.Add("Customer id is required");
        }

        if (request.Amount <= 0)
        {
            errors.Add("Amount must be greater than 0");
        }

        return await Task.FromResult((IReadOnlyCollection<string>)errors);
    }
}