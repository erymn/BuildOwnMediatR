namespace CustomImplementation.CommandHandler;

public sealed class CreateOrderHandler
{
    public Task<Guid> Handl(CreateOrderCommand command, CancellationToken cancellationToken = default)
    {
        //persist and return GuidID
        return Task.FromResult(Guid.NewGuid());
    }
}

public sealed record CreateOrderCommand(Guid CustomerId, decimal Amount);