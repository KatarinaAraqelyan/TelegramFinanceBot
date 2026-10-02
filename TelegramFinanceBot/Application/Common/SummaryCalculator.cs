using TelegramFinanceBot.DTOs;

namespace TelegramFinanceBot.Application.Common;

public static class SummaryCalculator
{
    public const int DailyRows = 14;

    public static DateTime GetToday(TimeProvider timeProvider) =>
        timeProvider.GetUtcNow().UtcDateTime.Date;

    public static DateTime GetMonthStart(DateTime today) =>
        new(today.Year, today.Month, 1, 0, 0, 0, DateTimeKind.Utc);

    public static SpendingSummary BuildSummary(IReadOnlyList<SpendingPoint> window, DateTime today)
    {
        var monthStart = GetMonthStart(today);
        var tomorrow = today.AddDays(1);

        var month = window
            .Where(point => point.SpentAt >= monthStart && point.SpentAt < tomorrow)
            .ToList();

        var monthTotal = month.Sum(point => point.Amount);
        var last7DaysTotal = SumBetween(window, today.AddDays(-7), today);
        var previous7DaysTotal = SumBetween(window, today.AddDays(-14), today.AddDays(-7));
        var typicalDay = monthTotal / today.Day;

        var categories = month
            .GroupBy(point => point.Category)
            .Select(group => new CategoryTotal(group.Key, group.Sum(point => point.Amount)))
            .OrderByDescending(category => category.Total)
            .ThenBy(category => category.Category)
            .ToList();

        return new SpendingSummary(
            monthTotal,
            month.Count,
            last7DaysTotal,
            previous7DaysTotal,
            typicalDay,
            categories);
    }

    public static IReadOnlyList<DailyTotal> BuildDailyTotals(IReadOnlyList<SpendingPoint> window, DateTime today)
    {
        var totalsByDay = window
            .GroupBy(point => point.SpentAt.Date)
            .ToDictionary(group => group.Key, group => group.Sum(point => point.Amount));

        return Enumerable.Range(0, DailyRows)
            .Select(offset => today.AddDays(offset - (DailyRows - 1)))
            .Select(day => new DailyTotal(DateOnly.FromDateTime(day), totalsByDay.GetValueOrDefault(day)))
            .ToList();
    }

    private static decimal SumBetween(IReadOnlyList<SpendingPoint> window, DateTime from, DateTime to) =>
        window
            .Where(point => point.SpentAt >= from && point.SpentAt < to)
            .Sum(point => point.Amount);
}