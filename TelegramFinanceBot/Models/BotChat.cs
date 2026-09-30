namespace TelegramFinanceBot.Models;

public sealed class BotChat
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public long TelegramChatId { get; set; }

    public string ReportToken { get; set; } = string.Empty;

    public DateTime StartedAt { get; set; }

    public List<Spending> Spendings { get; set; } = [];
}
