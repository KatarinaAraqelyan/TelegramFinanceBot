using Microsoft.Extensions.Options;
using TelegramFinanceBot.Configuration;
using TelegramFinanceBot.Services;

namespace TelegramFinanceBot.Workers;

public sealed class DailyDigestWorker(
    IServiceScopeFactory scopeFactory,
    IOptions<TelegramOptions> options,
    TimeProvider timeProvider,
    ILogger<DailyDigestWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var target = GetNextRun();

        logger.LogInformation("Next daily message at {Target:u}.", target);

        while (!stoppingToken.IsCancellationRequested)
        {
            await DelayUntilAsync(target, stoppingToken);

            try
            {
                await using var scope = scopeFactory.CreateAsyncScope();
                var digest = scope.ServiceProvider.GetRequiredService<IDigestService>();
                await digest.SendDailyAsync(stoppingToken);
            }
            catch (Exception exception) when (exception is not OperationCanceledException)
            {
                logger.LogError(exception, "Daily message run failed.");
            }

            target = target.AddDays(1);
        }
    }

    private DateTime GetNextRun()
    {
        var now = timeProvider.GetUtcNow().UtcDateTime;
        var next = now.Date.AddHours(options.Value.DigestHourUtc);

        return next <= now ? next.AddDays(1) : next;
    }

    private async Task DelayUntilAsync(DateTime target, CancellationToken cancellationToken)
    {
        while (true)
        {
            var remaining = target - timeProvider.GetUtcNow().UtcDateTime;

            if (remaining <= TimeSpan.Zero)
            {
                return;
            }

            var step = remaining > TimeSpan.FromHours(1) ? TimeSpan.FromHours(1) : remaining;

            await Task.Delay(step, cancellationToken);
        }
    }
}
