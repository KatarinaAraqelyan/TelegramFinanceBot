namespace TelegramFinanceBot.Application.Common;

public sealed record SpendingPoint(DateTime SpentAt, decimal Amount, string Category);