using TelegramFinanceBot.DTOs;

namespace TelegramFinanceBot.Services;

public interface IReportRenderer
{
    string Render(ReportDto report);
}
