using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TelegramFinanceBot.Configuration;
using TelegramFinanceBot.Controllers;

namespace TelegramFinanceBot.Workers;

public sealed class WebhookRegistrationWorker(
    ITelegramBotClient botClient,
    IOptions<TelegramOptions> telegramOptions,
    IOptions<PublicUrlOptions> publicUrlOptions,
    ILogger<WebhookRegistrationWorker> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var baseUrl = publicUrlOptions.Value.PublicBaseUrl.TrimEnd('/');

        if (!baseUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                "PublicBaseUrl must be a public HTTPS address (for example your ngrok URL) to register the Telegram webhook.");
        }

        var webhookUrl = $"{baseUrl}/{TelegramWebhookController.RoutePath}";

        await botClient.SetWebhook(
            url: webhookUrl,
            allowedUpdates: [UpdateType.Message],
            dropPendingUpdates: true,
            secretToken: telegramOptions.Value.WebhookSecret,
            cancellationToken: cancellationToken);

        logger.LogInformation("Telegram webhook registered: {WebhookUrl}", webhookUrl);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
