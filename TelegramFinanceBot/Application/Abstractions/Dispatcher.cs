namespace TelegramFinanceBot.Application.Abstractions;

public sealed class Dispatcher(IServiceProvider serviceProvider) : IDispatcher
{
    public async Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
        dynamic handler = serviceProvider.GetRequiredService(handlerType);

        return await handler.HandleAsync((dynamic)command, cancellationToken);
    }

    public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        dynamic handler = serviceProvider.GetRequiredService(handlerType);

        return await handler.HandleAsync((dynamic)query, cancellationToken);
    }
}
