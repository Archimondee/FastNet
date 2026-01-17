namespace Application.Users;

public sealed class CreateUserValidator
	: AbstractValidator<CreateUserCommand>
{
	public CreateUserValidator()
	{
		RuleFor(x => x.Email)
			.NotEmpty()
			.EmailAddress();

		RuleFor(x => x.Password)
			.NotEmpty()
			.MinimumLength(8);
	}
}