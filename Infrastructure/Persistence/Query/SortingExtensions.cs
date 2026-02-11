using System.Linq.Expressions;

namespace Infrastructure.Persistence.Query;

public static class SortingExtensions
{
  public static IQueryable<T> ApplySorting<T>(
    this IQueryable<T> query,
    string sort)
  {
    var parts = sort.Split(',');

    foreach (var part in parts)
    {
      var trimmed = part.Trim();
      var desc = trimmed.StartsWith("-");
      var property = desc ? trimmed[1..] : trimmed;

      var param = Expression.Parameter(typeof(T), "e");
      var body = Expression.PropertyOrField(param, property);
      var lambda = Expression.Lambda(body, param);

      var method = desc ? "OrderByDescending" : "OrderBy";

      query = (IQueryable<T>)typeof(Queryable)
        .GetMethods()
        .Single(m => m.Name == method
                     && m.GetParameters().Length == 2)
        .MakeGenericMethod(typeof(T), body.Type)
        .Invoke(null, new object[] { query, lambda })!;
    }

    return query;
  }
}
