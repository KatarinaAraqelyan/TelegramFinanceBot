namespace TelegramFinanceBot.DTOs;

public sealed record ParsedSpending(decimal Amount, string Category, string? Note);
