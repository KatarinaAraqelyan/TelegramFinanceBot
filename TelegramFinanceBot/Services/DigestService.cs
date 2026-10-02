using TelegramFinanceBot.Application.Abstractions;
using TelegramFinanceBot.Application.Chats;
using TelegramFinanceBot.Application.Spendings;
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
        var recipients = await dispatcher.QueryAsync(new GetDigestRecipientsQuery(), cancellationToken);

        logger.LogInformation("Sending daily message to {Count} chats.", recipients.Count);

        foreach (var recipient in recipients)
        {
            try
            {
                var summary = await dispatcher.QueryAsync(new GetSummaryQuery(recipient.ChatId), cancellationToken);
                var text = texts.Recap(summary, links.Build(recipient.ReportToken));

                await sender.SendAsync(recipient.TelegramChatId, text, cancellationToken);
            }
            catch (Exception exception) when (exception is not OperationCanceledException)
            {
                logger.LogError(exception, "Daily message failed for chat {ChatId}.", recipient.ChatId);
            }
        }
    }
}