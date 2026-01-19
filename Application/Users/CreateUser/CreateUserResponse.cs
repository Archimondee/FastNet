using Shared.Commons.Response;

namespace Application.Users.CreateUser;

public sealed class CreateUserResponse : BaseResponse
{
    public new Guid Id { get; set; }

    public string Email { get; set; } = default!;
}