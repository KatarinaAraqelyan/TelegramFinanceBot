namespace TelegramFinanceBot.DTOs;

public sealed record SpendingItem(DateTime SpentAt, decimal Amount, string Category, string? Note);
