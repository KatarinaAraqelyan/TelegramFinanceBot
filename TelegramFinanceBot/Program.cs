using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using TelegramFinanceBot.Configuration;
using TelegramFinanceBot.Data;
using TelegramFinanceBot.Repositories;
using TelegramFinanceBot.Telegram;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<TelegramOptions>()
    .Bind(builder.Configuration.GetSection(TelegramOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddOptions<PublicUrlOptions>()
    .Bind(builder.Configuration)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddDbContext<AppDbContext>(dbOptions =>
    dbOptions.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddSingleton<ITelegramBotClient>(serviceProvider =>
{
    var telegramOptions = serviceProvider.GetRequiredService<IOptions<TelegramOptions>>().Value;

    return new TelegramBotClient(telegramOptions.BotToken);
});

builder.Services.AddSingleton<TelegramMessageSender>();

builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<ISpendingRepository, SpendingRepository>();

builder.Services.AddScoped<TelegramUpdateHandler>();

builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.EnsureCreated();
}

app.MapControllers();

await app.RunAsync();