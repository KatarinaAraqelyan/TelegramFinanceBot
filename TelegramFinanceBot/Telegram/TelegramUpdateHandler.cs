using Telegram.Bot.Types;
using TelegramFinanceBot.Telegram.Commands;

namespace TelegramFinanceBot.Telegram;

public sealed class TelegramUpdateHandler(TelegramMessageSender sender, CommandRouter router)
{
    public async Task HandleAsync(Update update, CancellationToken cancellationToken)
    {
        var message = update.Message;

        if (message?.Text is null)
        {
            return;
        }

        var text = message.Text.Trim();

        if (text.Length == 0)
        {
            return;
        }

        var chatId = message.Chat.Id;
        var handler = router.Resolve(text);
        var reply = await handler.HandleAsync(new CommandContext(chatId, text), cancellationToken);

        await sender.SendAsync(chatId, reply, cancellationToken);
    }
}