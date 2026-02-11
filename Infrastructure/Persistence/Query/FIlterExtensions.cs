using System.Linq.Expressions;
using System.Reflection;

namespace Infrastructure.Persistence.Query;

public static class FilterExtensions
{
  public static IQueryable<T> ApplyFilters<T>(
    this IQueryable<T> query,
    IEnumerable<string> filters)
  {
    foreach (var raw in filters)
    {
      var condition = FilterParser.Parse(raw);
      var expression = BuildExpression<T>(condition);
      query = query.Where(expression);
    }

    return query;
  }

  private static Expression<Func<T, bool>> BuildExpression<T>(
    FilterCondition condition)
  {
    var parameter = Expression.Parameter(typeof(T), "e");
    var members = condition.Field.Split('.');

    Expression current = parameter;
    Type currentType = typeof(T);

    for (int i = 0; i < members.Length; i++)
    {
      var prop = currentType.GetProperty(
                   members[i],
                   BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                 ?? throw new InvalidOperationException($"Property '{members[i]}' not found.");

      // COLLECTION DETECTED
      if (typeof(System.Collections.IEnumerable).IsAssignableFrom(prop.PropertyType)
          && prop.PropertyType != typeof(string))
      {
        var elementType = prop.PropertyType.GetGenericArguments()[0];
        var collectionExpr = Expression.Property(current, prop);

        var innerParam = Expression.Parameter(elementType, "x");

        Expression innerCurrent = innerParam;
        Type innerType = elementType;

        // Navigate remaining members
        for (int j = i + 1; j < members.Length; j++)
        {
          var innerProp = innerType.GetProperty(
                            members[j],
                            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                          ?? throw new InvalidOperationException($"Property '{members[j]}' not found.");

          innerCurrent = Expression.Property(innerCurrent, innerProp);
          innerType = innerProp.PropertyType;
        }

        var comparison = BuildComparison(innerCurrent, innerType, condition);

        var lambda = Expression.Lambda(comparison, innerParam);

        return Expression.Lambda<Func<T, bool>>(
          Expression.Call(
            typeof(Enumerable),
            "Any",
            new[] { elementType },
            collectionExpr,
            lambda),
          parameter);
      }

      current = Expression.Property(current, prop);
      currentType = prop.PropertyType;
    }

    var finalComparison = BuildComparison(current, currentType, condition);

    return Expression.Lambda<Func<T, bool>>(finalComparison, parameter);
  }

  private static Expression BuildComparison(
    Expression left,
    Type type,
    FilterCondition condition)
  {
    var targetType = Nullable.GetUnderlyingType(type) ?? type;

    object value = targetType.IsEnum
      ? Enum.Parse(targetType, condition.Value, true)
      : Convert.ChangeType(condition.Value, targetType);

    Expression constant = Expression.Constant(value, targetType);

    // Handle nullable types
    if (type != targetType)
      constant = Expression.Convert(constant, type);

    return condition.Operator switch
    {
      "==" => Expression.Equal(left, constant),
      "!=" => Expression.NotEqual(left, constant),

      ">" => Expression.GreaterThan(left, constant),
      "<" => Expression.LessThan(left, constant),
      ">=" => Expression.GreaterThanOrEqual(left, constant),
      "<=" => Expression.LessThanOrEqual(left, constant),

      "@=" => Expression.Call(left, nameof(string.Contains), null, constant),
      "!@=" => Expression.Not(
        Expression.Call(left, nameof(string.Contains), null, constant)),

      "_=" => Expression.Call(left, nameof(string.StartsWith), null, constant),
      "=_ " => Expression.Call(left, nameof(string.EndsWith), null, constant),

      _ => throw new NotSupportedException(
        $"Operator '{condition.Operator}' is not supported.")
    };
  }
}
