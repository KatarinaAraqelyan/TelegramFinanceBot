using TelegramFinanceBot.Application.Abstractions;
using TelegramFinanceBot.Application.Chats;

namespace TelegramFinanceBot.Telegram.Commands;

public sealed class StartCommandHandler(IDispatcher dispatcher, MessageTextBuilder texts) : ICommandHandler
{
    public async Task<string> HandleAsync(CommandContext context, CancellationToken cancellationToken)
    {
        await dispatcher.SendAsync(new StartChatCommand(context.ChatId), cancellationToken);

        return texts.Start();
    }
}