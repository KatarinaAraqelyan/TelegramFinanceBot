using Microsoft.Extensions.Options;
using TelegramFinanceBot.Configuration;
using TelegramFinanceBot.DTOs;
using TelegramFinanceBot.Helpers;

namespace TelegramFinanceBot.Telegram;

public sealed class MessageTextBuilder(IOptions<TelegramOptions> options)
{
    private const string FormatLine = "Format: <amount> <category> [note], for example: 4.50 coffee";

    private string Currency => options.Value.Currency;

    public string Start() =>
        string.Join('\n',
            "Hello! 👋",
            "",
            "I'm your personal finance consultant. 💰",
            "",
            "Send me your spending, one per message, in this format:",
            "<amount> <category> [note]",
            "",
            "Examples:",
            "4.50 coffee",
            "32.10 groceries lidl",
            "120 rent",
            "",
            "Commands:",
            "/today - today's spendings",
            "/month - this month's recap",
            "",
            "Every day I'll send you a short recap with a link to a detailed report.");

    public string FormatReminder() => FormatLine;

    public string Invalid(string reason) => $"{reason} {FormatLine}";

    public string StartFirst() => "Send /start first.";

    public string Saved(ParsedSpending spending) =>
        $"Saved {MoneyFormatter.Format(spending.Amount)} {Currency} · {spending.Category}";

    public string NoSpendingsThisMonth() => "No spendings this month yet.";

    public string Today(TodayDto today)
    {
        if (today.Items.Count == 0)
        {
            return "No spendings today yet.";
        }

        var lines = new List<string>
        {
            $"Today: {MoneyFormatter.Format(today.Total)} {Currency} · {today.Items.Count} entries",
            ""
        };

        foreach (var item in today.Items)
        {
            var line = $"{item.SpentAt:HH:mm} {MoneyFormatter.Format(item.Amount)} {item.Category}";

            if (!string.IsNullOrEmpty(item.Note))
            {
                line += $" {item.Note}";
            }

            lines.Add(line);
        }

        return string.Join('\n', lines);
    }

    public string Recap(SpendingSummary summary, string link)
    {
        var lines = new List<string>
        {
            $"📊 This month: {MoneyFormatter.Format(summary.MonthTotal)} {Currency} · {summary.MonthCount} entries",
            $"Last 7 days: {MoneyFormatter.Format(summary.Last7DaysTotal)} {Currency}",
            $"Previous 7 days: {MoneyFormatter.Format(summary.Previous7DaysTotal)} {Currency}",
            $"Typical day: {MoneyFormatter.Format(summary.TypicalDay)} {Currency}",
            "",
            "Top categories:"
        };

        var rank = 1;

        foreach (var category in summary.Categories.Take(3))
        {
            lines.Add($"{rank++}. {category.Category} - {MoneyFormatter.Format(category.Total)} {Currency}");
        }

        lines.Add("");
        lines.Add($"Full report: {link}");

        return string.Join('\n', lines);
    }
}
