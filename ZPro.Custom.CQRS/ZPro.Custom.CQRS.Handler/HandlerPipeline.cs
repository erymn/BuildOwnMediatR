using ZPro.Custom.CQRS.Interface;

namespace ZPro.Custom.CQRS.Handler;

public sealed class HandlerPipeline<TRequest, TResponse> : IHandlerPipeline<TRequest, TResponse>
{
    private readonly IServiceProvider _serviceProvider;
    public HandlerPipeline(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    private readonly List<Type> _middlewares = new();
    private Func<TRequest, CancellationToken, Task<TResponse>>? _handler;
    
    public IHandlerPipeline<TRequest, TResponse> UseMiddleware<TMiddleware>() where TMiddleware : IHandlerPipeline<TRequest, TResponse>
    {
        _middlewares.Add(typeof(TMiddleware));
        return this;
    }

    public IHandlerPipeline<TRequest, TResponse> Run(Func<TRequest, CancellationToken, Task<TResponse>> handler)
    {
        _handler = handler;
        return this;
    }

    public Func<TRequest, CancellationToken, Task<TResponse>> Build()
    {
        if (_handler is null)
        {
            throw new InvalidOperationException("Handler was not configured. Call Run(...) before Build().");
        }

        return async (request, cancellationToken) =>
        {
            RequestHandlerDelegate<TResponse> next = () => _handler(request, cancellationToken);
            for (int i = _middlewares.Count - 1; i >= 0; i--)
            {
                var middlewareType = _middlewares[i];
                var middleware = (IHandlerMiddleware<TRequest, TResponse>)_serviceProvider.GetService(middlewareType)!;

                var currentNext = next;
                next = () => middleware.Handle(request, cancellationToken, currentNext);
            }

            return await next();
        };
    }
}