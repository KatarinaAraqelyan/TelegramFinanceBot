using Microsoft.Extensions.Options;
using TelegramFinanceBot.Configuration;

namespace TelegramFinanceBot.Services;

public sealed class ReportLinkBuilder(IOptions<PublicUrlOptions> options) : IReportLinkBuilder
{
    public string Build(string token) =>
        $"{options.Value.PublicBaseUrl.TrimEnd('/')}/report/{token}";
}
