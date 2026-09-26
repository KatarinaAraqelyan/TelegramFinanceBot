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

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        ReceiverOptions receiverOptions = new()
        {
            AllowedUpdates = [UpdateType.Message],
            DropPendingUpdates = true
        };

        _logger.LogInformation("Starting Telegram polling worker...");

        _botClient.StartReceiving(
            updateHandler: _updateHandler.HandleAsync,
            errorHandler: HandleErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: stoppingToken
        );

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Telegram polling error.");

        return Task.CompletedTask;
    }
}