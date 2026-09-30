using TelegramFinanceBot.Application.Abstractions;

namespace TelegramFinanceBot.Application;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IDispatcher, Dispatcher>();

        var handlerTypes = typeof(ApplicationExtensions).Assembly
            .GetTypes()
            .Where(type => type is { IsAbstract: false, IsInterface: false });

        foreach (var type in handlerTypes)
        {
            foreach (var contract in type.GetInterfaces().Where(IsHandlerContract))
            {
                services.AddScoped(contract, type);
            }
        }

        return services;
    }

    private static bool IsHandlerContract(Type type) =>
        type.IsGenericType &&
        (type.GetGenericTypeDefinition() == typeof(ICommandHandler<,>) ||
         type.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));
}
