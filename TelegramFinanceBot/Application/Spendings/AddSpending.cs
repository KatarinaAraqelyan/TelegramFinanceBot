using TelegramFinanceBot.Application.Abstractions;
using TelegramFinanceBot.Models;
using TelegramFinanceBot.Repositories;

namespace TelegramFinanceBot.Application.Spendings;

public sealed record AddSpendingCommand(Guid ChatId, decimal Amount, string Category, string? Note)
    : ICommand<Guid>;

public sealed class AddSpendingCommandHandler(ISpendingRepository spendings, TimeProvider timeProvider)
    : ICommandHandler<AddSpendingCommand, Guid>
{
    public async Task<Guid> HandleAsync(AddSpendingCommand command, CancellationToken cancellationToken)
    {
        var spending = new Spending
        {
            ChatId = command.ChatId,
            Amount = command.Amount,
            Category = command.Category,
            Note = command.Note,
            SpentAt = timeProvider.GetUtcNow().UtcDateTime
        };

        await spendings.AddAsync(spending, cancellationToken);

        return spending.Id;
    }
}