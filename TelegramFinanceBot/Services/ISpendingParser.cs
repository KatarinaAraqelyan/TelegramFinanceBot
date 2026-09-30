using TelegramFinanceBot.DTOs;

namespace TelegramFinanceBot.Services;

public interface ISpendingParser
{
    ParseResult Parse(string text);
}
