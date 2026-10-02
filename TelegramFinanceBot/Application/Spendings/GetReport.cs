using Microsoft.EntityFrameworkCore;
using TelegramFinanceBot.Application.Abstractions;
using TelegramFinanceBot.Application.Common;
using TelegramFinanceBot.Data;
using TelegramFinanceBot.DTOs;

namespace TelegramFinanceBot.Application.Spendings;

public sealed record GetReportQuery(Guid ChatId) : IQuery<ReportDto>;

public sealed class GetReportQueryHandler(AppDbContext dbContext, TimeProvider timeProvider)
    : IQueryHandler<GetReportQuery, ReportDto>
{
    private const int LatestCount = 20;

    public async Task<ReportDto> HandleAsync(GetReportQuery query, CancellationToken cancellationToken)
    {
        var today = SummaryCalculator.GetToday(timeProvider);
        var window = await SpendingWindowLoader.LoadAsync(dbContext, query.ChatId, today, cancellationToken);

        var latest = await dbContext.Spendings
            .AsNoTracking()
            .Where(spending => spending.ChatId == query.ChatId)
            .OrderByDescending(spending => spending.SpentAt)
            .Take(LatestCount)
            .Select(spending => new SpendingItem(spending.SpentAt, spending.Amount, spending.Category, spending.Note))
            .ToListAsync(cancellationToken);

        return new ReportDto(
            SummaryCalculator.BuildSummary(window, today),
            SummaryCalculator.BuildDailyTotals(window, today),
            latest);
    }
}