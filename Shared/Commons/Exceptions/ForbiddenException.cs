using Microsoft.AspNetCore.Http;
using Shared.Commons.Error;

namespace Shared.Commons.Exceptions;
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
