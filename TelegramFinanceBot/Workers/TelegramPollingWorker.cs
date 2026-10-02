using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramFinanceBot.Telegram;

namespace TelegramFinanceBot.Workers;

public sealed class TelegramPollingWorker(
    ITelegramBotClient botClient,
    IServiceScopeFactory scopeFactory,
    ILogger<TelegramPollingWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await botClient.DeleteWebhook(dropPendingUpdates: true, cancellationToken: stoppingToken);

        logger.LogInformation("Telegram long polling started.");

        await botClient.ReceiveAsync(
            HandleUpdateAsync,
            HandleErrorAsync,
            new ReceiverOptions { AllowedUpdates = [UpdateType.Message] },
            stoppingToken);
    }

    private async Task HandleUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cancellationToken)
    {
        try
        {
            await using var scope = scopeFactory.CreateAsyncScope();
            var updateHandler = scope.ServiceProvider.GetRequiredService<TelegramUpdateHandler>();

            await updateHandler.HandleAsync(update, cancellationToken);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            logger.LogError(exception, "Failed to handle update {UpdateId}.", update.Id);
        }
    }

    private Task HandleErrorAsync(ITelegramBotClient _, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Telegram polling error.");

        return Task.CompletedTask;
    }
}