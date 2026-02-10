using Application.Behavior;
using Application.Users.DeleteUser;
using FastEndpoints;
using Shared.Commons.Response;

namespace API.Endpoints.User;

public class DeleteUserEndpoint : Endpoint<EmptyRequest, ApiResponse<object>>
{
  private readonly DeleteUserHandler _handler;
  private readonly BehaviorExecutor<EmptyRequest, object> _executor;

  public DeleteUserEndpoint(
    DeleteUserHandler handler,
    BehaviorExecutor<EmptyRequest, object> executor)
  {
    _handler = handler;
    _executor = executor;
  }

  public override void Configure()
  {
    RoutePrefixOverride("api/v1");
    Delete("users/{id}");
    Roles("Admin", "User");
    Options(o => o.WithMetadata(new RequirePermissionAttribute("users.delete")));
    Summary(s =>
    {
      s.Summary = "Delete a user";
      s.Response<object>(200, "User successfully deleted");
      s.Response(400, "Validation error");
    });
  }

  public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
  {
    var userId = Route<string>("id");
    if (userId is null)
    {
      throw new BadHttpRequestException("User not found");
    }

    await _executor.ExecuteAsync(
      req,
      ct,
      () => _handler.Handle(userId, ct));

    await Send.OkAsync(
      new ApiResponse<object>
      {
        Success = true,
        Data = new { }
      },
      ct);
  }
}