using Application.Common.Listing;
using Application.Interface;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Query;

public sealed class EfListQueryProcessor<TEntity>
  : IListQueryProcessor<TEntity>
  where TEntity : class
{
  public async Task<PagedResult<TEntity>> ExecuteAsync(
    IQueryable<TEntity> source,
    ListQuery query,
    CancellationToken ct)
  {
    if (query.Filters is { Count: > 0 })
      source = source.ApplyFilters(query.Filters);

    if (!string.IsNullOrWhiteSpace(query.Sort))
      source = source.ApplySorting(query.Sort);

    var total = await source.CountAsync(ct);

    var items = await source
      .Skip((query.Page - 1) * query.PageSize)
      .Take(query.PageSize)
      .ToListAsync(ct);

    return new PagedResult<TEntity>(
      items,
      total,
      query.Page,
      query.PageSize);
  }
}
