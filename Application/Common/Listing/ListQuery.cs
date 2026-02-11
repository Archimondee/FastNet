namespace Application.Common.Listing;

public sealed class ListQuery
{
  public int Page { get; init; } = 1;

  public int PageSize { get; init; } = 10;

  public IReadOnlyList<string>? Filters { get; init; }

  public string? Sort { get; init; }
}