namespace Shared.Commons.Error;

public abstract class AppException : Exception
{
    public string Code { get; }

    public int StatusCode { get; }

    protected AppException(
        string code,
        string message,
        int statusCode)
        : base(message)
    {
        Code = code;
        StatusCode = statusCode;
    }
}

//if (!currentUser.IsAdmin)
  //  throw new ForbiddenException();

// if (user is null)
   // throw new NotFoundException("User");

// if (emailExists)
   // throw new ConflictException("Email already exists");