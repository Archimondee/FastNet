using Application.Common.Listing;

namespace Application.Interface;

public interface IListQueryProcessor<TEntity>
{
  Task<PagedResult<TEntity>> ExecuteAsync(
    IQueryable<TEntity> source,
    ListQuery query,
    CancellationToken ct);
}