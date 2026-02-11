namespace Infrastructure.Persistence.Query;

public static class FilterOperators
{
  public const string Equals = "==";

  public const string NotEquals = "!=";

  public const string Contains = "@=";

  public const string NotContains = "!@=";

  public static readonly string[] All =
  {
    "!@=", "==", "!=", "@="
  };
}