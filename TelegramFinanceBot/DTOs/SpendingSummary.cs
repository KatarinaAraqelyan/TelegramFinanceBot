namespace TelegramFinanceBot.DTOs;

public sealed record SpendingSummary(
    decimal MonthTotal,
    int MonthCount,
    decimal Last7DaysTotal,
    decimal Previous7DaysTotal,
    decimal TypicalDay,
    IReadOnlyList<CategoryTotal> Categories);
