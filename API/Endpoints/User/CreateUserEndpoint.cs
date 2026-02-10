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

    public CreateUserEndpoint(
        CreateUserHandler handler,
        BehaviorExecutor<DomainUser,
            DomainUser> executor)
    {
        _handler = handler;
        _executor = executor;
    }

    public override void Configure()
    {
        RoutePrefixOverride("api/v1");
        Post("/users/create");
        Roles("Admin", "User");
        Options(o => o.WithMetadata(new RequirePermissionAttribute("users.create")));
        Summary(s =>
        {
            s.Summary = "Create a new user";
            s.Response<CreateUserResponse>(200, "User successfully created");
            s.Response(400, "Validation error");
        });
    }

    public override async Task HandleAsync(
        CreateUserRequest req,
        CancellationToken ct)
    {
        var user = Map.ToEntity(req);

        var created = await _executor.ExecuteAsync(
            user,
            ct,
            () => _handler.Handle(user, req.Role, ct));

        const int time = 0;

        var response = Map.FromEntity(created);
        var apiResponse = ApiResponse<CreateUserResponse>.Ok(response, time);

        await Send.OkAsync(apiResponse, ct);
    }
}