# TelegramFinanceBot

A Telegram bot that tracks your spendings and sends you a daily recap.

Send a spending as a message, for example `4.50 coffee`. The bot saves it, and every day
it sends a short summary with a link to a detailed web report.

## Commands

| Command | What it does |
|---|---|
| `/start` | Registers the chat and explains the format |
| `/today` | Today's total and list |
| `/month` | This month's recap with a link to the full report |
| `<amount> <category> [note]` | Saves a spending, e.g. `32.10 groceries lidl` |

## Daily recap

Once a day at a configured UTC hour, every chat with spendings this month gets:

- month total and number of entries
- last 7 days and previous 7 days
- typical day (month total ÷ days passed)
- top 3 categories
- a link to the full report

The report page (`/report/{token}`) also shows all categories, the last 14 days
and the last 20 spendings.

## Tech stack

.NET 10 · ASP.NET Core · Telegram.Bot (webhook) · PostgreSQL + EF Core · Razor · ngrok

## How to run

You need: .NET 10 SDK, Docker, ngrok, and a bot token from [@BotFather](https://t.me/BotFather).

**1. Start the database**

    docker run -d --name financebot-db -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=financebot -p 5432:5432 postgres:16

**2. Start ngrok** in a separate terminal and copy the `https://...` address

    ngrok http 5108

**3. Set secrets** from the folder with `TelegramFinanceBot.csproj`

    cd TelegramFinanceBot
    dotnet user-secrets set "ConnectionStrings:Default" "Host=localhost;Port=5432;Database=financebot;Username=postgres;Password=postgres"
    dotnet user-secrets set "Telegram:BotToken" "<your bot token>"
    dotnet user-secrets set "Telegram:WebhookSecret" "$(openssl rand -hex 32)"
    dotnet user-secrets set "PublicBaseUrl" "https://<your-address>.ngrok-free.app"

**4. Run**

    dotnet run --launch-profile http

Then send `/start` to your bot.

## Settings

Currency and digest hour are in `appsettings.json`:

    "Telegram": {
      "Currency": "AMD",
      "DigestHourUtc": 18
    }

Never commit your bot token. It belongs in user-secrets only.
