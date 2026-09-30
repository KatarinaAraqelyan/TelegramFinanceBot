using TelegramFinanceBot.Application.Abstractions;
// using TelegramFinanceBot.Application.Chats;
// using TelegramFinanceBot.Application.Spendings;
using TelegramFinanceBot.Services;

namespace TelegramFinanceBot.Telegram.Commands;

public sealed class MonthCommandHandler(
    IDispatcher dispatcher,
    IReportLinkBuilder links,
    MessageTextBuilder texts) : ICommandHandler
{
    public async Task<string> HandleAsync(CommandContext context, CancellationToken cancellationToken)
    {
        // Needed:
        // GetChatByTelegramIdQuery
        // GetSummaryQuery
        return string.Empty; // placeholder
    }
}
