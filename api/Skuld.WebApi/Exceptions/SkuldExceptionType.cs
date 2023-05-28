namespace Skuld.WebApi.Exceptions
{
	public enum SkuldExceptionType
	{
		// General
		BadFormatId = 1,
		ValidationFailed = 2,
		JsonPatchException = 3,

		// User
		UserAlreadyExist = 100,
		UserLoginFailed = 101,
		UserNotFound = 102,
		UserUpdateFailed = 103,

		// RefreshToken
		RefreshTokenInvalid = 200,
	}
}
