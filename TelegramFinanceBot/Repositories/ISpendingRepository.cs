using TelegramFinanceBot.Models;

namespace TelegramFinanceBot.Repositories;

public interface ISpendingRepository
{
    Task AddAsync(Spending spending, CancellationToken cancellationToken);
}
