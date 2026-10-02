using Microsoft.EntityFrameworkCore;
using TelegramFinanceBot.Application.Abstractions;
using TelegramFinanceBot.Application.Common;
using TelegramFinanceBot.Data;
using TelegramFinanceBot.DTOs;

namespace TelegramFinanceBot.Application.Chats;

public sealed record GetDigestRecipientsQuery : IQuery<IReadOnlyList<DigestRecipientDto>>;

public sealed class GetDigestRecipientsQueryHandler(AppDbContext dbContext, TimeProvider timeProvider)
    : IQueryHandler<GetDigestRecipientsQuery, IReadOnlyList<DigestRecipientDto>>
{
    public async Task<IReadOnlyList<DigestRecipientDto>> HandleAsync(
        GetDigestRecipientsQuery query,
        CancellationToken cancellationToken)
    {
        var monthStart = SummaryCalculator.GetMonthStart(SummaryCalculator.GetToday(timeProvider));

        return await dbContext.Chats
            .AsNoTracking()
            .Where(chat => chat.Spendings.Any(spending => spending.SpentAt >= monthStart))
            .Select(chat => new DigestRecipientDto(chat.Id, chat.TelegramChatId, chat.ReportToken))
            .ToListAsync(cancellationToken);
    }
}