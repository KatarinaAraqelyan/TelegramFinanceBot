using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramFinanceBot.Telegram;

public sealed class TelegramUpdateHandler(TelegramMessageSender messageSender)
{
    private readonly TelegramMessageSender _messageSender = messageSender;

    public async Task HandleAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is null || string.IsNullOrWhiteSpace(update.Message.Text))
        {
            return;
        }

        var chatId = update.Message.Chat.Id;
        var text = update.Message.Text.Trim();

        string responseText;

        if (text == "/start")
        {
            responseText =
                "Hello! 👋\n\n" +
                "I'm your personal finance consultant. 💰\n\n" +
                "Send me your spending in this format:\n" +
                "<amount> <category> [note]\n\n" +
                "Examples:\n" +
                "4.50 coffee\n" +
                "32.10 groceries lidl\n" +
                "120 rent\n\n" +
                "I'll help you track your spending and give you daily and monthly summaries.";
        }
        else
        {
            responseText = $"We received your message: {text}";
        }
        await _messageSender.SendAsync(chatId, responseText, cancellationToken);
    }
    
}