using Microsoft.EntityFrameworkCore;
using TelegramFinanceBot.Data;

namespace TelegramFinanceBot.Application.Common;

public static class SpendingWindowLoader
{
    public static async Task<IReadOnlyList<SpendingPoint>> LoadAsync(
        AppDbContext dbContext,
        Guid chatId,
        DateTime today,
        CancellationToken cancellationToken)
    {
        var monthStart = SummaryCalculator.GetMonthStart(today);
        var twoWeeksAgo = today.AddDays(-SummaryCalculator.DailyRows);
        var from = monthStart < twoWeeksAgo ? monthStart : twoWeeksAgo;
        var to = today.AddDays(1);

        return await dbContext.Spendings
            .AsNoTracking()
            .Where(spending => spending.ChatId == chatId
                               && spending.SpentAt >= from
                               && spending.SpentAt < to)
            .Select(spending => new SpendingPoint(spending.SpentAt, spending.Amount, spending.Category))
            .ToListAsync(cancellationToken);
    }
}