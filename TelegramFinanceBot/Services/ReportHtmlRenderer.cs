using System.Globalization;
using System.Net;
using System.Text;
using Microsoft.Extensions.Options;
using TelegramFinanceBot.Configuration;
using TelegramFinanceBot.DTOs;
using TelegramFinanceBot.Helpers;

namespace TelegramFinanceBot.Services;

public sealed class ReportHtmlRenderer(IOptions<TelegramOptions> options) : IReportRenderer
{
    private const string Styles = """
        :root { color-scheme: light dark; font-family: system-ui, -apple-system, "Segoe UI", sans-serif; }
        body { max-width: 760px; margin: 0 auto; padding: 16px; line-height: 1.4; }
        h1 { font-size: 1.5rem; margin: 0 0 16px; }
        h2 { font-size: 1.1rem; margin: 28px 0 8px; }
        .cards { display: grid; grid-template-columns: repeat(auto-fit, minmax(160px, 1fr)); gap: 10px; }
        .card { border: 1px solid rgba(128, 128, 128, 0.4); border-radius: 10px; padding: 10px 12px; }
        .card .label { font-size: 0.8rem; opacity: 0.7; }
        .card .value { font-size: 1.2rem; font-weight: 600; }
        .card .hint { font-size: 0.8rem; opacity: 0.7; }
        .scroll { overflow-x: auto; }
        table { border-collapse: collapse; width: 100%; }
        th, td { text-align: left; padding: 6px 8px; border-bottom: 1px solid rgba(128, 128, 128, 0.3); }
        td.num, th.num { text-align: right; white-space: nowrap; }
        .muted { opacity: 0.7; }
        """;

    public string Render(ReportDto report)
    {
        var currency = Encode(options.Value.Currency);
        var summary = report.Summary;
        var sb = new StringBuilder();

        sb.Append("<!DOCTYPE html><html lang=\"en\"><head><meta charset=\"utf-8\">");
        sb.Append("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">");
        sb.Append("<meta name=\"robots\" content=\"noindex, nofollow\">");
        sb.Append("<title>Spending report</title><style>").Append(Styles).Append("</style></head><body>");
        sb.Append("<h1>Spending report</h1>");

        sb.Append("<div class=\"cards\">");
        AppendCard(sb, "This month", $"{MoneyFormatter.Format(summary.MonthTotal)} {currency}", $"{summary.MonthCount} entries");
        AppendCard(sb, "Last 7 days", $"{MoneyFormatter.Format(summary.Last7DaysTotal)} {currency}", null);
        AppendCard(sb, "Previous 7 days", $"{MoneyFormatter.Format(summary.Previous7DaysTotal)} {currency}", null);
        AppendCard(sb, "Typical day", $"{MoneyFormatter.Format(summary.TypicalDay)} {currency}", "month total / days elapsed");
        sb.Append("</div>");

        sb.Append("<h2>Categories this month</h2>");

        if (summary.Categories.Count == 0)
        {
            sb.Append("<p class=\"muted\">No spendings this month.</p>");
        }
        else
        {
            sb.Append("<div class=\"scroll\"><table><thead><tr><th>Category</th><th class=\"num\">Total</th><th class=\"num\">Share</th></tr></thead><tbody>");

            foreach (var category in summary.Categories)
            {
                var share = summary.MonthTotal > 0 ? category.Total / summary.MonthTotal * 100 : 0;
                sb.Append("<tr><td>").Append(Encode(category.Category)).Append("</td>");
                sb.Append("<td class=\"num\">").Append(MoneyFormatter.Format(category.Total)).Append(' ').Append(currency).Append("</td>");
                sb.Append("<td class=\"num\">").Append(share.ToString("0.0", CultureInfo.InvariantCulture)).Append("%</td></tr>");
            }

            sb.Append("</tbody></table></div>");
        }

        sb.Append("<h2>Last 14 days (UTC)</h2>");
        sb.Append("<div class=\"scroll\"><table><thead><tr><th>Date</th><th class=\"num\">Total</th></tr></thead><tbody>");

        foreach (var day in report.Days.Reverse())
        {
            sb.Append("<tr><td>").Append(day.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)).Append("</td>");
            sb.Append("<td class=\"num\">").Append(MoneyFormatter.Format(day.Total)).Append(' ').Append(currency).Append("</td></tr>");
        }

        sb.Append("</tbody></table></div>");

        sb.Append("<h2>Last 20 spendings (UTC)</h2>");

        if (report.Latest.Count == 0)
        {
            sb.Append("<p class=\"muted\">No spendings yet.</p>");
        }
        else
        {
            sb.Append("<div class=\"scroll\"><table><thead><tr><th>Date</th><th class=\"num\">Amount</th><th>Category</th><th>Note</th></tr></thead><tbody>");

            foreach (var item in report.Latest)
            {
                sb.Append("<tr><td>").Append(item.SpentAt.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)).Append("</td>");
                sb.Append("<td class=\"num\">").Append(MoneyFormatter.Format(item.Amount)).Append(' ').Append(currency).Append("</td>");
                sb.Append("<td>").Append(Encode(item.Category)).Append("</td>");
                sb.Append("<td>").Append(Encode(item.Note ?? string.Empty)).Append("</td></tr>");
            }

            sb.Append("</tbody></table></div>");
        }

        sb.Append("</body></html>");

        return sb.ToString();
    }

    private static void AppendCard(StringBuilder sb, string label, string value, string? hint)
    {
        sb.Append("<div class=\"card\"><div class=\"label\">").Append(label).Append("</div>");
        sb.Append("<div class=\"value\">").Append(value).Append("</div>");

        if (hint is not null)
        {
            sb.Append("<div class=\"hint\">").Append(hint).Append("</div>");
        }

        sb.Append("</div>");
    }

    private static string Encode(string value) => WebUtility.HtmlEncode(value);
}
