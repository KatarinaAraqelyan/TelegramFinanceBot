using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramFinanceBot.Configuration;
using TelegramFinanceBot.Telegram;

namespace TelegramFinanceBot.Controllers;

[ApiController]
[Route(RoutePath)]
public sealed class TelegramWebhookController(
    TelegramUpdateHandler updateHandler,
    IOptions<TelegramOptions> options,
    ILogger<TelegramWebhookController> logger) : ControllerBase
{
    public const string RoutePath = "telegram/webhook";

    private const string SecretHeader = "X-Telegram-Bot-Api-Secret-Token";

    [HttpPost]
    public async Task<IActionResult> Post(CancellationToken cancellationToken)
    {
        if (!IsSecretValid())
        {
            return Unauthorized();
        }

        Update? update;

        try
        {
            update = await JsonSerializer.DeserializeAsync<Update>(Request.Body, JsonBotAPI.Options, cancellationToken);
        }
        catch (JsonException)
        {
            return BadRequest();
        }

        if (update is null)
        {
            return BadRequest();
        }

        try
        {
            await updateHandler.HandleAsync(update, cancellationToken);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            logger.LogError(exception, "Failed to handle update {UpdateId}.", update.Id);
        }

        return Ok();
    }

    private bool IsSecretValid()
    {
        if (!Request.Headers.TryGetValue(SecretHeader, out var provided))
        {
            return false;
        }

        var expectedBytes = Encoding.UTF8.GetBytes(options.Value.WebhookSecret);
        var providedBytes = Encoding.UTF8.GetBytes(provided.ToString());

        return CryptographicOperations.FixedTimeEquals(expectedBytes, providedBytes);
    }
}
