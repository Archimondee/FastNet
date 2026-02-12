namespace Shared.Commons.Documentation;

public static class SwaggerDocs
{
  public const string ListFilteringDescription = """
                                                 Filtering & Sorting

                                                 Operators:
                                                 ==   Equals
                                                 !=   Not equals
                                                 >    Greater than
                                                 >=   Greater or equal
                                                 <    Less than
                                                 <=   Less or equal
                                                 @=   Contains
                                                 !@=  Not contains
                                                 _=   Starts with
                                                 =_   Ends with

                                                 Examples:
                                                 email@=admin
                                                 isActive==true
                                                 userRoles.role.name==Admin

                                                 Multiple filters:
                                                 ?filters=email@=admin&filters=isActive==true

                                                 Sorting:
                                                 ?sort=email,-createdAt
                                                 """;
}