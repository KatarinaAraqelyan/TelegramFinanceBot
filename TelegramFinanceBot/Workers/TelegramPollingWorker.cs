using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using TelegramFinanceBot.Telegram;

namespace TelegramFinanceBot.Workers;

public sealed class TelegramPollingWorker(
    ITelegramBotClient botClient,
    TelegramUpdateHandler updateHandler,
    ILogger<TelegramPollingWorker> logger) : BackgroundService
{
    private readonly ITelegramBotClient _botClient = botClient;
    private readonly TelegramUpdateHandler _updateHandler = updateHandler;
    private readonly ILogger<TelegramPollingWorker> _logger = logger;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }

    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Telegram polling error.");

        return Task.CompletedTask;
    }
}