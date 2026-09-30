using TelegramFinanceBot.Application.Abstractions;
// using TelegramFinanceBot.Application.Chats;
// using TelegramFinanceBot.Application.Spendings;

namespace TelegramFinanceBot.Telegram.Commands;

public sealed class TodayCommandHandler(IDispatcher dispatcher, MessageTextBuilder texts) : ICommandHandler
{
    public async Task<string> HandleAsync(CommandContext context, CancellationToken cancellationToken)
    {
        // Needed:
        // GetChatByTelegramIdQuery
        // GetTodayQuery
        return string.Empty;
    }
}
