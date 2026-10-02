using TelegramFinanceBot.Application.Abstractions;
using TelegramFinanceBot.Application.Chats;
using TelegramFinanceBot.Application.Spendings;

namespace TelegramFinanceBot.Telegram.Commands;

public sealed class TodayCommandHandler(IDispatcher dispatcher, MessageTextBuilder texts) : ICommandHandler
{
    public async Task<string> HandleAsync(CommandContext context, CancellationToken cancellationToken)
    {
        var chat = await dispatcher.QueryAsync(new GetChatByTelegramIdQuery(context.ChatId), cancellationToken);

        if (chat is null)
        {
            return texts.StartFirst();
        }

        var today = await dispatcher.QueryAsync(new GetTodayQuery(chat.Id), cancellationToken);

        return texts.Today(today);
    }
}