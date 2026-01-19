using Application.Interface;

namespace Application.Behavior;

public sealed class BehaviorExecutor<TRequest, TResponse>
{
    private readonly IReadOnlyList<IBehavior<TRequest, TResponse>> _behaviors;

    public BehaviorExecutor(IEnumerable<IBehavior<TRequest, TResponse>> behaviors)
    {
        _behaviors = behaviors.Reverse().ToList();
    }

    public Task<TResponse> ExecuteAsync(
        TRequest request,
        CancellationToken ct,
        Func<Task<TResponse>> handler)
    {
        Func<Task<TResponse>> pipeline = handler;

        foreach (var behavior in _behaviors)
        {
            var next = pipeline;
            pipeline = () => behavior.Handle(request, ct, next);
        }

        return pipeline();
    }
}