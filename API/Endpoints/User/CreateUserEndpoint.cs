using Application.Users.Create;
using FastEndpoints;

namespace API.Endpoints.User;

public sealed class CreateUserEndpoint
    : Endpoint<CreateUserCommand, object>
{
    private readonly CreateUserHandler _handler;

    public CreateUserEndpoint(CreateUserHandler handler)
    {
        _handler = handler;
    }

    public override void Configure()
    {
        Post("/api/users");
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        CreateUserCommand req,
        CancellationToken ct)
    {
        var id = await _handler.Handle(req, ct);

        await Send.OkAsync(new { id }, ct);
    }
}