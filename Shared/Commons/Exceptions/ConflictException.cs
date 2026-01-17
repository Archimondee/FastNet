using Microsoft.AspNetCore.Http;
using Shared.Commons.Error;

namespace Shared.Commons.Exceptions;

public sealed class ConflictException : AppException
{
	public ConflictException(string message)
		: base(
			ErrorCodes.Conflict,
			message,
			StatusCodes.Status409Conflict)
	{
	}
}
