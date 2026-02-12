using Application.Behavior;
using Application.Common.Listing;
using Application.Users.GetUsers;
using FastEndpoints;
using Shared.Commons.Documentation;
using Shared.Commons.Response;

namespace API.Endpoints.User;

public class GetUsersEndpoint : Endpoint<ListQuery, ApiResponse<PagedResult<GetUserResponse>>>
{
  private readonly GetUserHandler _handler;
  private readonly BehaviorExecutor<ListQuery, PagedResult<GetUserResponse>> _executor;

  public GetUsersEndpoint(
    GetUserHandler handler,
    BehaviorExecutor<ListQuery, PagedResult<GetUserResponse>> executor)
  {
    _handler = handler;
    _executor = executor;
  }

  public override void Configure()
  {
    RoutePrefixOverride("api/v1");
    Get("/users");
    Options(o => o.WithMetadata(new RequirePermissionAttribute("*")));
    Summary(s =>
    {
      s.Summary = "Get users";
      s.Description = SwaggerDocs.ListFilteringDescription;
      s.Response<List<GetUserResponse>>(200, "Users successfully retrieved");
    });
  }

  public override async Task HandleAsync(ListQuery req, CancellationToken ct)
  {
    var result = await _executor.ExecuteAsync(
      req,
      ct,
      () => _handler.Handle(req, ct));

    const int time = 0;
    var apiResponse = ApiResponse<PagedResult<GetUserResponse>>.Ok(result, time);

    await Send.OkAsync(apiResponse, ct);
  }
}