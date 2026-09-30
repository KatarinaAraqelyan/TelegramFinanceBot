namespace TelegramFinanceBot.Telegram.Commands;

public sealed record CommandContext(long ChatId, string Text);
