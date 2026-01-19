using Application.Behavior;
using Application.Users.CreateUser;
using FastEndpoints;
using Shared.Commons.Response;
using DomainUser = Domain.Entities.User;

namespace API.Endpoints.User;

public sealed class CreateUserEndpoint
    : Endpoint<CreateUserRequest, ApiResponse<CreateUserResponse>, CreateUserMapper>
{
    private readonly CreateUserHandler _handler;
    private readonly BehaviorExecutor<DomainUser, DomainUser> _executor;

    public CreateUserEndpoint(CreateUserHandler handler, BehaviorExecutor<DomainUser, DomainUser> executor)
    {
        _handler = handler;
        _executor = executor;
    }

    public override void Configure()
    {
        Post("/api/v1/users");
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        CreateUserRequest req,
        CancellationToken ct)
    {
        var user = Map.ToEntity(req);

        var created = await _executor.ExecuteAsync(
        user,
        ct,
        () => _handler.Handle(user, ct));

        const int time = 0;

        var response = Map.FromEntity(created);
        var apiResponse = ApiResponse<CreateUserResponse>.Ok(response, time);

        await Send.OkAsync(apiResponse, ct);
    }
}