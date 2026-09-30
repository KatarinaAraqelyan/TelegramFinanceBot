namespace TelegramFinanceBot.Telegram.Commands;

public sealed class UnknownCommandHandler(MessageTextBuilder texts) : ICommandHandler
{
    public Task<string> HandleAsync(CommandContext context, CancellationToken cancellationToken) =>
        Task.FromResult(texts.FormatReminder());
}
