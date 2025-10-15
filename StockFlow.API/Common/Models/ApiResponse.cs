namespace StockFlow.Common.Models;

public sealed class ApiResponse
{
    public string Message { get; set; } = string.Empty;

    public bool Success { get; set; }

    public object? Errors { get; set; }

    public object? Data { get; set; }

    public int StatusCode { get; set; }
    public static ApiResponse SuccessResponse(
            string message,
            object? data = null,
            int statusCode = 200)
    {
        return new ApiResponse
        {
            Success = true,
            StatusCode = statusCode,
            Message = message,
            Data = data
        };
    }

    public static ApiResponse FailResponse(
        string message,
        int statusCode = 400,
        object? errors = null)
    {
        return new ApiResponse
        {
            Success = false,
            StatusCode = statusCode,
            Message = message,
            Errors = errors
        };
    }
}