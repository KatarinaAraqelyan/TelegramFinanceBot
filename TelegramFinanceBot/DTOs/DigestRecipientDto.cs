namespace TelegramFinanceBot.DTOs;

public sealed record DigestRecipientDto(Guid ChatId, long TelegramChatId, string ReportToken);
