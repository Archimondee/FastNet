namespace Application.Common.Listing;

public sealed class ListQuery
{
  /// <summary>Page number (starting from 1)</summary>
  /// <example>1</example>
  public int Page { get; set; } = 1;

  /// <summary>Number of records per page</summary>
  /// <example>10</example>
  public int PageSize { get; set; } = 10;

  /// <summary>Sorting expression</summary>
  /// <example>email</example>
  public string? Sort { get; set; }

  /// <summary>
  /// Filter: field operator value
  ///
  /// Operators:
  ///
  /// @= contains
  ///
  /// == equals
  ///
  /// != not-equals
  ///
  /// _= starts-with
  ///
  /// =_ ends-with
  ///
  /// >&gt; &lt; &gt;= &lt;= numeric/date comparison
  ///
  /// Examples:
  /// email@=admin
  /// isActive==true
  /// userRoles.role.name==Admin
  /// </summary>
  public List<string>? Filters { get; set; }
}