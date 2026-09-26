## Telegram Bot API Setup

This project uses the Telegram Bot API to communicate with Telegram.

### 1. Create a Telegram Bot

Open **@BotFather** in Telegram and send:

```text
/newbot
```

Follow the instructions to create a bot.

At the end, BotFather will provide a **Bot Token**.

> ⚠️ Keep the token private. Do not commit it to GitHub or put it directly in the source code.

### 2. Configure the Bot Token

This project uses **ASP.NET Core User Secrets** for local development.

From the folder containing the `.csproj` file, run:

```bash
dotnet user-secrets init
```

Then add your own Bot Token:

```bash
dotnet user-secrets set "Telegram:BotToken" "YOUR_BOT_TOKEN"
```

Replace `YOUR_BOT_TOKEN` with the token you received from BotFather.

You can check that it was saved with:

```bash
dotnet user-secrets list
```

### 3. Run the Project

After configuring the token:

```bash
dotnet restore
dotnet run
```

The application will start the Telegram bot using long polling.
