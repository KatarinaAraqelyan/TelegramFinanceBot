using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TelegramFinanceBot.Application.Abstractions;
// using TelegramFinanceBot.Application.Chats;
// using TelegramFinanceBot.Application.Spendings;
using TelegramFinanceBot.Configuration;
using TelegramFinanceBot.ViewModels;

namespace TelegramFinanceBot.Controllers;

[AllowAnonymous]
[Route("report")]
public sealed class ReportController(IDispatcher dispatcher, IOptions<TelegramOptions> options) : Controller
{
    [HttpGet("{token}")]
    public async Task<IActionResult> Index(string token, CancellationToken cancellationToken)
    {
        // Needed:
        // GetChatByTokenQuery
        // GetReportQuery
        return Ok(); // placeholder
        
        // var chat = await dispatcher.QueryAsync(new GetChatByTokenQuery(token), cancellationToken);
        //
        // if (chat is null)
        // {
        //     return NotFound();
        // }
        //
        // var report = await dispatcher.QueryAsync(new GetReportQuery(chat.Id), cancellationToken);
        //
        // Response.Headers.CacheControl = "no-store";
        // Response.Headers["Referrer-Policy"] = "no-referrer";
        //
        // return View(new ReportViewModel(report, options.Value.Currency));
    }
}
