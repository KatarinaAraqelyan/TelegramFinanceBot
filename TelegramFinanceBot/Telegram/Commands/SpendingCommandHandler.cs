using TelegramFinanceBot.Application.Abstractions;
using TelegramFinanceBot.Application.Chats;
using TelegramFinanceBot.Application.Spendings;
using TelegramFinanceBot.Services;

namespace TelegramFinanceBot.Telegram.Commands;

public sealed class SpendingCommandHandler(
    IDispatcher dispatcher,
    ISpendingParser parser,
    MessageTextBuilder texts) : ICommandHandler
{
    public async Task<string> HandleAsync(CommandContext context, CancellationToken cancellationToken)
    {
        var chat = await dispatcher.QueryAsync(new GetChatByTelegramIdQuery(context.ChatId), cancellationToken);

        if (chat is null)
        {
            return texts.StartFirst();
        }

        var result = parser.Parse(context.Text);

        if (!result.IsSuccess)
        {
            return texts.Invalid(result.Error!);
        }

        var spending = result.Value!;

        await dispatcher.SendAsync(
            new AddSpendingCommand(chat.Id, spending.Amount, spending.Category, spending.Note),
            cancellationToken);

        return texts.Saved(spending);
    }
}