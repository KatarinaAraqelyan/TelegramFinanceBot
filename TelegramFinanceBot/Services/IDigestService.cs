namespace TelegramFinanceBot.Services;

public interface IDigestService
{
    Task SendDailyAsync(CancellationToken cancellationToken);
}
