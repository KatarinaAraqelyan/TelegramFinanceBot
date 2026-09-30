using System.Globalization;
using TelegramFinanceBot.DTOs;

namespace TelegramFinanceBot.Services;

public sealed class SpendingParser : ISpendingParser
{
    private const decimal MaxAmount = 1_000_000_000_000m;

    public ParseResult Parse(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return ParseResult.Failure("Message is empty.");
        }

        if (text.Contains('\n') || text.Contains('\r'))
        {
            return ParseResult.Failure("Send one spending per message, on a single line.");
        }

        var parts = text.Trim().Split((char[]?)null, 3, StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < 2)
        {
            return ParseResult.Failure("I need an amount and a category.");
        }

        var rawAmount = parts[0].Replace(',', '.');

        if (!decimal.TryParse(rawAmount, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var amount))
        {
            return ParseResult.Failure("The first word must be an amount like 12.50 or 12,50, without minus sign or currency symbol.");
        }

        amount = decimal.Round(amount, 2, MidpointRounding.AwayFromZero);

        if (amount <= 0)
        {
            return ParseResult.Failure("The amount must be greater than zero.");
        }

        if (amount > MaxAmount)
        {
            return ParseResult.Failure("The amount is too large.");
        }

        var category = parts[1];

        if (!category.All(char.IsAsciiLetter))
        {
            return ParseResult.Failure("The category must be one word made of letters only.");
        }

        var note = parts.Length == 3 ? parts[2].Trim() : null;

        if (string.IsNullOrEmpty(note))
        {
            note = null;
        }

        return ParseResult.Success(new ParsedSpending(amount, category.ToLowerInvariant(), note));
    }
}
