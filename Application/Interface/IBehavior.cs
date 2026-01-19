namespace Application.Interface;

public interface IBehavior<TRequest, TResponse>
{
    Task<TResponse> Handle(
        TRequest request,
        CancellationToken ct,
        Func<Task<TResponse>> next);
}