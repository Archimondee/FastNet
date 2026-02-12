namespace Application.Common.Listing;

public sealed class PagedResult<T>
{
  public IReadOnlyList<T> Data { get; }

  public int TotalItems { get; }

  public int Page { get; }

  public int PageSize { get; }

  public int LastPage { get; }

  public int? NextPage { get; }

  public int? PreviousPage { get; }

  public PagedResult(
    IReadOnlyList<T> data,
    int totalItems,
    int page,
    int pageSize)
  {
    Data = data;
    TotalItems = totalItems;
    Page = page;
    PageSize = pageSize;

    LastPage = (int)Math.Ceiling(totalItems / (double)pageSize);

    NextPage = page < LastPage ? page + 1 : null;
    PreviousPage = page > 1 ? page - 1 : null;
  }
}
