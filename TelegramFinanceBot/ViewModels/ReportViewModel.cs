using TelegramFinanceBot.DTOs;

namespace TelegramFinanceBot.ViewModels;

public sealed record ReportViewModel(ReportDto Report, string Currency);
