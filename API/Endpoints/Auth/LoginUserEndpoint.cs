using Application.Auth.LoginUser;
using Application.Behavior;
using FastEndpoints;
using Shared.Commons.Response;
using DomainUser = Domain.Entities.User;

namespace API.Endpoints.Auth;

public sealed class LoginUserEndpoint
  : Endpoint<LoginUserRequest, ApiResponse<LoginUserResponse>, LoginUserMapper>
{
  private readonly LoginUserHandler _handler;
  private readonly BehaviorExecutor<DomainUser, LoginUserResponse> _executor;

  public LoginUserEndpoint(
    LoginUserHandler handler,
    BehaviorExecutor<DomainUser,
      LoginUserResponse> executor)
  {
    _handler = handler;
    _executor = executor;
  }

  public override void Configure()
  {
    Post("auth/login");
    AllowAnonymous();

    Tags("Auth");
    Summary(s =>
    {
      s.Summary = "Login a user";
      s.Response<LoginUserResponse>(200, "User successfully logged in");
      s.Response(400, "Validation error");
    });
  }

  public override async Task HandleAsync(
    LoginUserRequest req,
    CancellationToken ct)
  {
    var user = Map.ToEntity(req);

    var created = await _executor.ExecuteAsync(
      user,
      ct,
      () => _handler.Handle(user, ct));

    const int time = 0;

    var apiResponse = ApiResponse<LoginUserResponse>.Ok(created, time);

    await Send.OkAsync(apiResponse, ct);
  }
}