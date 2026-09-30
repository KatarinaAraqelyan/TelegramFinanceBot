namespace TelegramFinanceBot.DTOs;

public sealed record ReportDto(
    SpendingSummary Summary,
    IReadOnlyList<DailyTotal> Days,
    IReadOnlyList<SpendingItem> Latest);
