using Microsoft.EntityFrameworkCore;
using TelegramFinanceBot.Application.Abstractions;
using TelegramFinanceBot.Application.Common;
using TelegramFinanceBot.Data;
using TelegramFinanceBot.DTOs;

namespace TelegramFinanceBot.Application.Spendings;

public sealed record GetTodayQuery(Guid ChatId) : IQuery<TodayDto>;

public sealed class GetTodayQueryHandler(AppDbContext dbContext, TimeProvider timeProvider)
    : IQueryHandler<GetTodayQuery, TodayDto>
{
    public async Task<TodayDto> HandleAsync(GetTodayQuery query, CancellationToken cancellationToken)
    {
        var today = SummaryCalculator.GetToday(timeProvider);
        var tomorrow = today.AddDays(1);

        var items = await dbContext.Spendings
            .AsNoTracking()
            .Where(spending => spending.ChatId == query.ChatId
                               && spending.SpentAt >= today
                               && spending.SpentAt < tomorrow)
            .OrderBy(spending => spending.SpentAt)
            .Select(spending => new SpendingItem(spending.SpentAt, spending.Amount, spending.Category, spending.Note))
            .ToListAsync(cancellationToken);

        return new TodayDto(items.Sum(item => item.Amount), items);
    }
}