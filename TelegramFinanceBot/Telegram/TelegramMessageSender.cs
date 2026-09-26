using Telegram.Bot;

namespace TelegramFinanceBot.Telegram;

public sealed class TelegramMessageSender(ITelegramBotClient botClient)
{
    private readonly ITelegramBotClient _botClient = botClient;

    public async Task SendAsync(long chatId, string text, CancellationToken cancellationToken = default)
    {
        await _botClient.SendMessage(chatId, text, cancellationToken: cancellationToken);
    }
}