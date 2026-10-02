using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using TelegramFinanceBot.Application;
using TelegramFinanceBot.Configuration;
using TelegramFinanceBot.Data;
using TelegramFinanceBot.Repositories;
using TelegramFinanceBot.Services;
using TelegramFinanceBot.Telegram;
using TelegramFinanceBot.Telegram.Commands;
using TelegramFinanceBot.Workers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<TelegramOptions>()
    .Bind(builder.Configuration.GetSection(TelegramOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddOptions<PublicUrlOptions>()
    .Bind(builder.Configuration)
    .ValidateDataAnnotations()
    .ValidateOnStart();

var connectionString = builder.Configuration.GetConnectionString("Default");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "ConnectionStrings:Default is not configured. Set it with dotnet user-secrets or an environment variable.");
}

builder.Services.AddDbContext<AppDbContext>(dbOptions => dbOptions.UseNpgsql(connectionString));

builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddSingleton<ITelegramBotClient>(serviceProvider =>
{
    var telegramOptions = serviceProvider.GetRequiredService<IOptions<TelegramOptions>>().Value;

    return new TelegramBotClient(telegramOptions.BotToken);
});

builder.Services.AddSingleton<TelegramMessageSender>();
builder.Services.AddSingleton<MessageTextBuilder>();
builder.Services.AddSingleton<ISpendingParser, SpendingParser>();
builder.Services.AddSingleton<IReportLinkBuilder, ReportLinkBuilder>();

builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<ISpendingRepository, SpendingRepository>();
builder.Services.AddScoped<IDigestService, DigestService>();

builder.Services.AddApplication();

builder.Services.AddScoped<CommandRouter>();
builder.Services.AddScoped<TelegramUpdateHandler>();

builder.Services.RegisterCommand<StartCommandHandler>(CommandKeys.Start);
builder.Services.RegisterCommand<TodayCommandHandler>(CommandKeys.Today);
builder.Services.RegisterCommand<MonthCommandHandler>(CommandKeys.Month);
builder.Services.RegisterCommand<SpendingCommandHandler>(CommandKeys.Spending);
builder.Services.RegisterCommand<UnknownCommandHandler>(CommandKeys.Unknown);

var updateMode = builder.Configuration.GetValue(
    $"{TelegramOptions.SectionName}:{nameof(TelegramOptions.UpdateMode)}",
    TelegramUpdateMode.Polling);

if (updateMode == TelegramUpdateMode.Webhook)
{
    builder.Services.AddHostedService<WebhookRegistrationWorker>();
}
else
{
    builder.Services.AddHostedService<TelegramPollingWorker>();
}

builder.Services.AddHostedService<DailyDigestWorker>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.EnsureCreated();
}

app.UseStaticFiles();
app.MapControllers();

await app.RunAsync();