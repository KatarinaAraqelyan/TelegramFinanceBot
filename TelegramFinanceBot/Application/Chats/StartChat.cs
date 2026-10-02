using System.Security.Cryptography;
using TelegramFinanceBot.Application.Abstractions;
using TelegramFinanceBot.Models;
using TelegramFinanceBot.Repositories;

namespace TelegramFinanceBot.Application.Chats;

public sealed record StartChatCommand(long TelegramChatId) : ICommand<StartChatResult>;

public sealed record StartChatResult(Guid ChatId, string ReportToken);

public sealed class StartChatCommandHandler(IChatRepository chats, TimeProvider timeProvider)
    : ICommandHandler<StartChatCommand, StartChatResult>
{
    public async Task<StartChatResult> HandleAsync(StartChatCommand command, CancellationToken cancellationToken)
    {
        var existing = await chats.GetByTelegramIdAsync(command.TelegramChatId, cancellationToken);

        if (existing is not null)
        {
            return new StartChatResult(existing.Id, existing.ReportToken);
        }

        var chat = new BotChat
        {
            TelegramChatId = command.TelegramChatId,
            ReportToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(16)).ToLowerInvariant(),
            StartedAt = timeProvider.GetUtcNow().UtcDateTime
        };

        await chats.AddAsync(chat, cancellationToken);

        return new StartChatResult(chat.Id, chat.ReportToken);
    }
}