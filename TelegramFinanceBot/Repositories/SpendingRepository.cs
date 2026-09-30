using TelegramFinanceBot.Data;
using TelegramFinanceBot.Models;

namespace TelegramFinanceBot.Repositories;

public sealed class SpendingRepository(AppDbContext dbContext) : ISpendingRepository
{
    public async Task AddAsync(Spending spending, CancellationToken cancellationToken)
    {
        dbContext.Spendings.Add(spending);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
