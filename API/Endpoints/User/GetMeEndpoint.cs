using System.Security.Claims;
using Application.Behavior;
using Application.Users.GetMe;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Shared.Commons.Exceptions;
using Shared.Commons.Response;
using DomainUser = Domain.Entities.User;

namespace API.Endpoints.User;

public class GetMeEndpoint : Endpoint<EmptyRequest, ApiResponse<GetMeResponse>, GetMeMapper>
{
  private readonly GetMeHandler _handler;
  private readonly BehaviorExecutor<DomainUser, DomainUser> _executor;

  public GetMeEndpoint(
    GetMeHandler handler,
    BehaviorExecutor<DomainUser, DomainUser> executor)
  {
    _handler = handler;
    _executor = executor;
  }

  public override void Configure()
  {
    RoutePrefixOverride("api/v1");
    Get("/users/me");
    Options(o => o.WithMetadata(new RequirePermissionAttribute("*")));
    Summary(s =>
    {
      s.Summary = "Get current user";

      s.Response<GetMeResponse>(200, "User successfully retrieved");
      s.Response(404, "User not found");
    });
  }

  public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
  {
    var userId = User.FindFirstValue(JwtRegisteredClaimNames.NameId);

    var user = await _executor.ExecuteAsync(
      null!,
      ct,
      () => userId != null ? _handler.Handle(userId, ct) : throw new NotFoundException("User not found"));

    const int time = 0;

    var response = Map.FromEntity(user);
    var apiResponse = ApiResponse<GetMeResponse>.Ok(response, time);

    await Send.OkAsync(apiResponse, ct);
  }
}