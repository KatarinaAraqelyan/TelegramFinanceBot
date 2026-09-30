using System.ComponentModel.DataAnnotations;

namespace TelegramFinanceBot.Configuration;

public sealed class TelegramOptions
{
    public const string SectionName = "Telegram";

    [Required]
    public string BotToken { get; set; } = string.Empty;

    [Required]
    [RegularExpression("^[A-Za-z0-9_-]{1,256}$")]
    public string WebhookSecret { get; set; } = string.Empty;

    [Required]
    public string Currency { get; set; } = "AMD";

    [Range(0, 23)]
    public int DigestHourUtc { get; set; } = 18;
}