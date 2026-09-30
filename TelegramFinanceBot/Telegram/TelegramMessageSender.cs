using Telegram.Bot;

namespace TelegramFinanceBot.Telegram;

public sealed class TelegramMessageSender(ITelegramBotClient botClient)
{
    public async Task SendAsync(long chatId, string text, CancellationToken cancellationToken = default)
    {
        await botClient.SendMessage(chatId, text, cancellationToken: cancellationToken);
    }
}