using Microsoft.AspNetCore.Http;
using Shared.Commons.Error;

namespace Shared.Commons.Exceptions;

public sealed class NotFoundException : AppException
{
    public NotFoundException(string resource)
        : base(
            ErrorCodes.NotFound,
            $"{resource} not found",
            StatusCodes.Status404NotFound)
    {
    }
}

// throw new NotFoundException("User");