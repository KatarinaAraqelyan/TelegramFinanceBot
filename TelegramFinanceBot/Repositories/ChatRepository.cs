using Microsoft.EntityFrameworkCore;
using TelegramFinanceBot.Data;
using TelegramFinanceBot.Models;

namespace TelegramFinanceBot.Repositories;

public sealed class ChatRepository(AppDbContext dbContext) : IChatRepository
{
    public Task<BotChat?> GetByTelegramIdAsync(long telegramChatId, CancellationToken cancellationToken) =>
        dbContext.Chats
            .AsNoTracking()
            .FirstOrDefaultAsync(chat => chat.TelegramChatId == telegramChatId, cancellationToken);

    public async Task AddAsync(BotChat chat, CancellationToken cancellationToken)
    {
        dbContext.Chats.Add(chat);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
