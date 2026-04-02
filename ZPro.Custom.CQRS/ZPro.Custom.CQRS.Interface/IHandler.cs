namespace ZPro.Custom.CQRS.Interface;

public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();

public interface IHandlerMiddleware<TRequest, TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next);
}

public interface IHandlerPipeline<TRequest, TResponse>
{
    IHandlerPipeline<TRequest, TResponse> UseMiddleware<TMiddleware>()
        where TMiddleware : IHandlerPipeline<TRequest, TResponse>;
    IHandlerPipeline<TRequest, TResponse> Run(Func<TRequest, CancellationToken, Task<TResponse>> func);
    Func<TRequest, CancellationToken, Task<TResponse>> Build();
}