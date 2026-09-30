using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TelegramFinanceBot.Application.Abstractions;
// using TelegramFinanceBot.Application.Chats;
// using TelegramFinanceBot.Application.Spendings;
using TelegramFinanceBot.Services;

namespace TelegramFinanceBot.Controllers;

[ApiController]
[AllowAnonymous]
[Route("report")]
public sealed class ReportController(IDispatcher dispatcher, IReportRenderer renderer) : ControllerBase
{
    [HttpGet("{token}")]
    public async Task<IActionResult> Get(string token, CancellationToken cancellationToken)
    {
        // Needed:
        // GetChatByTokenQuery
        // GetReportQuery
        return Ok(); // placeholder
    }
}
