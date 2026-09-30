namespace TelegramFinanceBot.Telegram.Commands;

public static class CommandRegistrationExtensions
{
    public static IServiceCollection RegisterCommand<T>(this IServiceCollection services, string key)
        where T : class, ICommandHandler =>
        services.AddKeyedScoped<ICommandHandler, T>(key);
}
