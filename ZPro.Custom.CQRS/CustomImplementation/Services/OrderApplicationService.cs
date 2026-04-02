using CustomImplementation.CommandHandler;
using ZPro.Custom.CQRS.Handler;
using ZPro.Custom.CQRS.Interface;

namespace CustomImplementation.Services;

public class OrderApplicationService
{
    private readonly IHandlerPipeline<CreateOrderCommand, Guid> _pipeline;
    private readonly CreateOrderHandler _handler;
 
    public OrderApplicationService(
        IHandlerPipeline<CreateOrderCommand, Guid> pipeline,
        CreateOrderHandler handler)
    {
        _pipeline = pipeline;
        _handler = handler;
    }
 
    public Task<Guid> Execute(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        return _pipeline
            .UseMiddleware<ValidatorMiddleware<CreateOrderCommand, Guid>>()
            .UseMiddleware<LoggingMiddleware<CreateOrderCommand, Guid>>()
            .Run((request, ct) => _handler.Handle(request, ct))
            .Build()
            .Invoke(command, cancellationToken);
    }
}