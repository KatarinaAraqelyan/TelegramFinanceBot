using TelegramFinanceBot.Application.Abstractions;
// using TelegramFinanceBot.Application.Chats;
// using TelegramFinanceBot.Application.Spendings;
using TelegramFinanceBot.Telegram;

namespace TelegramFinanceBot.Services;

public sealed class DigestService(
    IDispatcher dispatcher,
    IReportLinkBuilder links,
    MessageTextBuilder texts,
    TelegramMessageSender sender,
    ILogger<DigestService> logger) : IDigestService
{
    public async Task SendDailyAsync(CancellationToken cancellationToken)
    {
        // Needed:
        // GetDigestRecipientsQuery
        // GetSummaryQuery
    }
}
