namespace Skuld.Shared.Exceptions
{
    public enum SkuldExceptionType
    {
        // General
        BadFormatId = 1,
        ValidationFailed = 2,

        // User
        UserAlreadyExist = 100,
        UserLoginFailed = 101,
        UserNotFound = 102
    }
}
