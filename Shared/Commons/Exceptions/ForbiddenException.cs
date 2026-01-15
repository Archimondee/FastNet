using Microsoft.AspNetCore.Http;

namespace Shared.Commons.Error;
public sealed class ForbiddenException : AppException
{
    public ForbiddenException(string message = "Forbidden")
        : base(
            ErrorCodes.Forbidden,
            message,
            StatusCodes.Status403Forbidden)
    {
    }
}
