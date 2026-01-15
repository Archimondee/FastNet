using Microsoft.AspNetCore.Http;
using Shared.Commons.Error;

namespace Shared.Commons.Exceptions;

public sealed class UnauthorizedException : AppException
{
    public UnauthorizedException(string message = "Unauthorized")
        : base(
            ErrorCodes.Unauthorized,
            message,
            StatusCodes.Status401Unauthorized)
    {
    }
}