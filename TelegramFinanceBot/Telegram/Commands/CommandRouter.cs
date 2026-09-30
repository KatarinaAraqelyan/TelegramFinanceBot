namespace TelegramFinanceBot.Telegram.Commands;

public sealed class CommandRouter(IServiceProvider serviceProvider)
{
    public ICommandHandler Resolve(string text)
    {
        var key = text.StartsWith('/') ? ExtractCommand(text) : CommandKeys.Spending;

        return serviceProvider.GetKeyedService<ICommandHandler>(key)
            ?? serviceProvider.GetRequiredKeyedService<ICommandHandler>(CommandKeys.Unknown);
    }

    private static string ExtractCommand(string text)
    {
        var command = text.Split((char[]?)null, 2, StringSplitOptions.RemoveEmptyEntries)[0];
        var mention = command.IndexOf('@');

        if (mention > 0)
        {
            command = command[..mention];
        }

        return command.ToLowerInvariant();
    }
}
