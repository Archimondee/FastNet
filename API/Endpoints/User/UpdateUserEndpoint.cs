using Application.Behavior;
using Application.Users.UpdateUser;
using FastEndpoints;
using Shared.Commons.Response;
using DomainUser = Domain.Entities.User;

namespace API.Endpoints.User;

public sealed class UpdateUserEndpoint : Endpoint<UpdateUserRequest, ApiResponse<UpdateUserResponse>, UpdateUserMapper>
{
  private readonly UpdateUserHandler _handler;
  private readonly BehaviorExecutor<DomainUser, DomainUser> _executor;

  public UpdateUserEndpoint(
    UpdateUserHandler handler,
    BehaviorExecutor<DomainUser, DomainUser> executor)
  {
    _handler = handler;
    _executor = executor;
  }

  public override void Configure()
  {
    RoutePrefixOverride("api/v1");
    Put("/users/{id}");
    Roles("Admin", "User");
    Options(o => o.WithMetadata(new RequirePermissionAttribute("users.update")));
    Summary(s =>
    {
      s.Summary = "Update an existing user";
      s.Response<UpdateUserResponse>(200, "User successfully updated");
      s.Response(400, "Validation error");
    });
  }

  public override async Task HandleAsync(
    UpdateUserRequest req,
    CancellationToken ct)
  {
    var userId = Route<Guid>("id");
    var user = Map.ToEntity(req);

    var updated = await _executor.ExecuteAsync(
      user,
      ct,
      () => _handler.Handle(userId, user, ct));

    const int time = 0;

    var response = Map.FromEntity(updated);
    var apiResponse = ApiResponse<UpdateUserResponse>.Ok(response, time);

    await Send.OkAsync(apiResponse, ct);
  }
}