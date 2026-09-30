using TelegramFinanceBot.Models;

namespace TelegramFinanceBot.Repositories;

public interface IChatRepository
{
    Task<BotChat?> GetByTelegramIdAsync(long telegramChatId, CancellationToken cancellationToken);

    Task AddAsync(BotChat chat, CancellationToken cancellationToken);
}
