namespace TelegramFinanceBot.Telegram.Commands;

public interface ICommandHandler
{
    Task<string> HandleAsync(CommandContext context, CancellationToken cancellationToken);
}
