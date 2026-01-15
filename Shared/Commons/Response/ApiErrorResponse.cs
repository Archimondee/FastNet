namespace Shared.Commons.Response;

public sealed class ApiErrorResponse
{
    public string Code { get; init; } = default!;

    public string Message { get; init; } = default!;

    public string CorrelationId { get; init; } = default!;
}