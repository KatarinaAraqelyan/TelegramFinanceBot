using System.Globalization;

namespace TelegramFinanceBot.Helpers;

public static class MoneyFormatter
{
    public static string Format(decimal value) =>
        value.ToString("0.00", CultureInfo.InvariantCulture);
}
