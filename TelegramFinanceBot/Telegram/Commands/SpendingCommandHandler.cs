using TelegramFinanceBot.Application.Abstractions;
// using TelegramFinanceBot.Application.Chats;
// using TelegramFinanceBot.Application.Spendings;
using TelegramFinanceBot.Services;

namespace TelegramFinanceBot.Telegram.Commands;

public sealed class SpendingCommandHandler(
    IDispatcher dispatcher,
    ISpendingParser parser,
    MessageTextBuilder texts) : ICommandHandler
{
    public async Task<string> HandleAsync(CommandContext context, CancellationToken cancellationToken)
    {
        // Needed:
        // GetChatByTelegramIdQuery
        // AddSpendingCommand
        return string.Empty;
    }
}
