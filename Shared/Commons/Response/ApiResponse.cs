namespace Shared.Commons.Response;

public sealed class ApiResponse<T>
{
    public bool Success { get; init; }

    public long Time { get; init; }

    public T? Data { get; init; }

    public static ApiResponse<T> Ok(
        T data,
        long elapsedMs)
        => new()
        {
            Success = true,
            Time = elapsedMs,
            Data = data
        };
}