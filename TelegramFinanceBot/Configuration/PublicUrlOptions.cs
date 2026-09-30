using System.ComponentModel.DataAnnotations;

namespace TelegramFinanceBot.Configuration;

public sealed class PublicUrlOptions
{
    [Required]
    [Url]
    public string PublicBaseUrl { get; set; } = string.Empty;
}
