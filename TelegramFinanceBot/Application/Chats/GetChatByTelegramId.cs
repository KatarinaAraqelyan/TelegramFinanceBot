using Microsoft.EntityFrameworkCore;
using TelegramFinanceBot.Application.Abstractions;
using TelegramFinanceBot.Data;
using TelegramFinanceBot.DTOs;

namespace TelegramFinanceBot.Application.Chats;

public sealed record GetChatByTelegramIdQuery(long TelegramChatId) : IQuery<ChatInfoDto?>;

public sealed class GetChatByTelegramIdQueryHandler(AppDbContext dbContext)
    : IQueryHandler<GetChatByTelegramIdQuery, ChatInfoDto?>
{
    public Task<ChatInfoDto?> HandleAsync(GetChatByTelegramIdQuery query, CancellationToken cancellationToken) =>
        dbContext.Chats
            .AsNoTracking()
            .Where(chat => chat.TelegramChatId == query.TelegramChatId)
            .Select(chat => new ChatInfoDto(chat.Id, chat.ReportToken))
            .FirstOrDefaultAsync(cancellationToken);
}