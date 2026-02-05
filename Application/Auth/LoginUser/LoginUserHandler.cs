using FluentValidation;

namespace Application.Auth.LoginUser;

public class LoginUserHandler
{
}

public sealed class LoginUserValidator
  : AbstractValidator<LoginUserRequest>
{
  public LoginUserValidator()
  {
    RuleFor(x => x.Email)
      .NotEmpty()
      .EmailAddress();

    RuleFor(x => x.Password)
      .NotEmpty()
      .MinimumLength(8);
  }
}