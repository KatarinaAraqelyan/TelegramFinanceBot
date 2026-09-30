namespace TelegramFinanceBot.Models;

public sealed class Spending
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public Guid ChatId { get; set; }

    public BotChat Chat { get; set; } = null!;

    public decimal Amount { get; set; }

    public string Category { get; set; } = string.Empty;

    public string? Note { get; set; }

    public DateTime SpentAt { get; set; }
}
