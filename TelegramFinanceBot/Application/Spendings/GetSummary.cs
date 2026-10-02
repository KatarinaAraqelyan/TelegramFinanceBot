using TelegramFinanceBot.Application.Abstractions;
using TelegramFinanceBot.Application.Common;
using TelegramFinanceBot.Data;
using TelegramFinanceBot.DTOs;

namespace TelegramFinanceBot.Application.Spendings;

public sealed record GetSummaryQuery(Guid ChatId) : IQuery<SpendingSummary>;

public sealed class GetSummaryQueryHandler(AppDbContext dbContext, TimeProvider timeProvider)
    : IQueryHandler<GetSummaryQuery, SpendingSummary>
{
    public async Task<SpendingSummary> HandleAsync(GetSummaryQuery query, CancellationToken cancellationToken)
    {
        var today = SummaryCalculator.GetToday(timeProvider);
        var window = await SpendingWindowLoader.LoadAsync(dbContext, query.ChatId, today, cancellationToken);

        return SummaryCalculator.BuildSummary(window, today);
    }
}