namespace TelegramFinanceBot.DTOs;

public sealed record TodayDto(decimal Total, IReadOnlyList<SpendingItem> Items);
