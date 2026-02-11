namespace Infrastructure.Persistence.Query;

public sealed class FilterCondition
{
  public string Field { get; init; } = default!;

  public string Operator { get; init; } = default!;

  public string Value { get; init; } = default!;
}

public static class FilterParser
{
  public static FilterCondition Parse(string raw)
  {
    foreach (var op in FilterOperators.All)
    {
      var idx = raw.IndexOf(op);
      if (idx <= 0) continue;

      return new FilterCondition
      {
        Field = raw[..idx],
        Operator = op,
        Value = raw[(idx + op.Length)..]
      };
    }

    throw new ArgumentException($"Invalid filter: {raw}");
  }
}
