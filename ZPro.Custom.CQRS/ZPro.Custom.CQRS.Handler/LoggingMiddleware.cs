using Microsoft.Extensions.Logging;
using ZPro.Custom.CQRS.Interface;

namespace ZPro.Custom.CQRS.Handler;

public class LoggingMiddleware<TRequest, TResponse> : IHandlerMiddleware<TRequest, TResponse>
{
    private readonly ILogger<LoggingMiddleware<TRequest, TResponse>> _logger;

    public LoggingMiddleware(ILogger<LoggingMiddleware<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        _logger.LogInformation("Handling {RequestType}", typeof(TRequest).Name);
        var response = await next();
        _logger.LogInformation("Handled {ResponseType}", typeof(TResponse).Name);
        return response;
    }
}