using Microsoft.EntityFrameworkCore;
using TelegramFinanceBot.Application.Abstractions;
using TelegramFinanceBot.Data;
using TelegramFinanceBot.DTOs;

namespace TelegramFinanceBot.Application.Chats;

public sealed record GetChatByTokenQuery(string Token) : IQuery<ChatInfoDto?>;

public sealed class GetChatByTokenQueryHandler(AppDbContext dbContext)
    : IQueryHandler<GetChatByTokenQuery, ChatInfoDto?>
{
    public Task<ChatInfoDto?> HandleAsync(GetChatByTokenQuery query, CancellationToken cancellationToken) =>
        dbContext.Chats
            .AsNoTracking()
            .Where(chat => chat.ReportToken == query.Token)
            .Select(chat => new ChatInfoDto(chat.Id, chat.ReportToken))
            .FirstOrDefaultAsync(cancellationToken);
}