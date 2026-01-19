using Application.Interface;
using Microsoft.Extensions.Logging;

namespace Application.Behavior;

public sealed class TransactionBehavior<TRequest, TResponse>
    : IBehavior<TRequest, TResponse>
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

    public TransactionBehavior(
        IUnitOfWork uow,
        ILogger<TransactionBehavior<TRequest, TResponse>> logger)
    {
        _uow = uow;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        CancellationToken ct,
        Func<Task<TResponse>> next)
    {
        await _uow.BeginAsync(ct);

        try
        {
            var response = await next();

            await _uow.CommitAsync(ct);

            return response;
        }
        catch (Exception ex)
        {
            await _uow.RollbackAsync(ct);

            _logger.LogWarning(
                ex,
                "Transaction rolled back for {Request}",
                typeof(TRequest).Name);

            throw;
        }
    }
}