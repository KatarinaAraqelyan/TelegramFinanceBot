using TelegramFinanceBot.Application.Abstractions;
using TelegramFinanceBot.Application.Chats;
using TelegramFinanceBot.Application.Spendings;
using TelegramFinanceBot.Services;

namespace TelegramFinanceBot.Telegram.Commands;

public sealed class MonthCommandHandler(
    IDispatcher dispatcher,
    IReportLinkBuilder links,
    MessageTextBuilder texts) : ICommandHandler
{
    public async Task<string> HandleAsync(CommandContext context, CancellationToken cancellationToken)
    {
        var chat = await dispatcher.QueryAsync(new GetChatByTelegramIdQuery(context.ChatId), cancellationToken);

        if (chat is null)
        {
            return texts.StartFirst();
        }

        var summary = await dispatcher.QueryAsync(new GetSummaryQuery(chat.Id), cancellationToken);

        if (summary.MonthCount == 0)
        {
            return texts.NoSpendingsThisMonth();
        }

        return texts.Recap(summary, links.Build(chat.ReportToken));
    }
}