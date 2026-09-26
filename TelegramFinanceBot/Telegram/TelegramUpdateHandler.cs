using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramFinanceBot.Telegram;

public sealed class TelegramUpdateHandler(TelegramMessageSender messageSender)
{
    private readonly TelegramMessageSender _messageSender = messageSender;

    public async Task HandleAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}