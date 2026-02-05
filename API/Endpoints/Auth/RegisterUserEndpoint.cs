using Application.Auth.RegisterUser;
using Application.Behavior;
using FastEndpoints;
using Shared.Commons.Response;
using DomainUser = Domain.Entities.User;

namespace API.Endpoints.Auth;

public sealed class RegisterUserEndpoint
  : Endpoint<RegisterUserRequest, ApiResponse<RegisterUserResponse>, RegisterUserMapper>
{
  private readonly RegisterUserHandler _handler;
  private readonly BehaviorExecutor<DomainUser, DomainUser> _executor;

  public RegisterUserEndpoint(
    RegisterUserHandler handler,
    BehaviorExecutor<DomainUser,
      DomainUser> executor)
  {
    _handler = handler;
    _executor = executor;
  }

  public override void Configure()
  {
    RoutePrefixOverride("api/v1");
    Post("/auth/register");
    AllowAnonymous();

    Tags("Auth");
    Summary(s =>
    {
      s.Summary = "Register a new user";
      s.Response<RegisterUserResponse>(200, "User successfully registered");
      s.Response(400, "Validation error");
    });
  }

  public override async Task HandleAsync(
    RegisterUserRequest req,
    CancellationToken ct)
  {
    var user = Map.ToEntity(req);

    var created = await _executor.ExecuteAsync(
      user,
      ct,
      () => _handler.Handle(user, ct));

    const int time = 0;

    var response = Map.FromEntity(created);
    var apiResponse = ApiResponse<RegisterUserResponse>.Ok(response, time);

    await Send.OkAsync(apiResponse, ct);
  }
}