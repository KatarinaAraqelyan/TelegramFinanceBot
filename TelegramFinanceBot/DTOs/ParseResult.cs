namespace TelegramFinanceBot.DTOs;

public sealed record ParseResult(ParsedSpending? Value, string? Error)
{
    public bool IsSuccess => Value is not null;

    public static ParseResult Success(ParsedSpending value) => new(value, null);

    public static ParseResult Failure(string error) => new(null, error);
}
