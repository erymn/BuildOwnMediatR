using Microsoft.Extensions.DependencyInjection;
using ZPro.Custom.CQRS.Handler;
using ZPro.Custom.CQRS.Interface;

namespace ZPro.Custom.CQRS.DI;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddHandlerPipeline(this IServiceCollection services)
    {
        services.AddScoped(typeof(IHandlerPipeline<,>), typeof(IHandlerPipeline<,>));

        services.AddScoped(typeof(ValidatorMiddleware<,>));
        services.AddScoped(typeof(LoggingMiddleware<,>));

        return services;
    }
}