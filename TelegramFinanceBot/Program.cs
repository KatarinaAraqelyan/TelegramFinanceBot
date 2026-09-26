using Microsoft.Extensions.Options;
using Telegram.Bot;
using TelegramFinanceBot.Configuration;
using TelegramFinanceBot.Workers;
using TelegramFinanceBot.Telegram;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<TelegramOptions>(
    builder.Configuration.GetSection(TelegramOptions.SectionName));

builder.Services.AddSingleton<ITelegramBotClient>(serviceProvider =>
{
    var options = serviceProvider
        .GetRequiredService<IOptions<TelegramOptions>>()
        .Value;

    if (string.IsNullOrWhiteSpace(options.BotToken))
    {
        throw new InvalidOperationException(
            "Telegram bot token is not configured.");
    }

    return new TelegramBotClient(options.BotToken);
});

builder.Services.AddSingleton<TelegramMessageSender>();
builder.Services.AddSingleton<TelegramUpdateHandler>();

builder.Services.AddHostedService<TelegramPollingWorker>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

await app.RunAsync();