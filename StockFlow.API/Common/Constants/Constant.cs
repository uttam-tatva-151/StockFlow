namespace StockFlow.Common.Constants;
public static class Constant
{
    public static readonly string DefaultConnection = "DefaultConnection";
    public static readonly string AppDbConnection = "AppDbConnection";
    public static readonly string AppSettings = "AppSettings";

    public static readonly string Ascending = "asc";
    public static readonly string Descending = "desc";

    public const string Success = "success";
    public const string Error = "error";
    public const string Authorization = "Authorization";

    public static readonly string PDFFormat = "application/pdf";

    public static class ErrorMessages
    {
        public const string ResourceNotFound = "Resource not found.";
        public const string InternalServerError = "Internal Server Error.";
        public const string UnhandledException = "Unhandled exception occurred.";
        public const string InvalidRefreshToken = "Invalid Refresh Token";
        public const string InvalidCredentials = "Invalid credentials.";
    }

    public static class SuccessMessages
    {
        public const string AddedSuccessfully = "{0} added successfully";
        public const string UpdatedSuccessfully = "{0} updated successfully";
        public const string DeletedSuccessfully = "{0} deleted successfully";
        public const string LoginSuccessfully = "Login Successfully";
        public const string RegisterSuccessfully = "User registered successfully.";

    }
    public static class WarningMessages
    {
        public const string EmailAlreadyExists = "User with this email already exists.";
    }
    public static class JWT 
    {
        public const string InvalidAccessToken = "The provided access token is invalid or has expired. Please log in again.";
        public const string MissingAccessToken = "Access token is missing from the request. Please include a valid token in the Authorization header.";
        public const string InvalidRefreshToken = "The refresh token is invalid or has expired. Please request a new login session.";
        public const string TokenValidationFailed = "Token validation failed due to invalid credentials or expired session.";
        public const string Bearer = "Bearer ";
        public const string RefreshTokenHeader = "X-Refresh-Token";
        public const string RefreshToken = "RefreshToken";
        public const string AccessToken = "AcessToken";

    }

    public static class ContentTypes
    {
        public const string ApplicationJson = "application/json";
    }


}
