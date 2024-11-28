using revisifyBackened.Models.Dto;

namespace revisifyBackened.Utilities
{
    public static class ApiResponseHelper
    {
        public static ApiResponse<T> CreateSuccessResponse<T>(T data, string message)
        {
            return new ApiResponse<T>(true, message, data, StatusCodes.Status200OK);
        }

        public static ApiResponse<T> CreateErrorResponse<T>(string message, int statusCode)
        {
            return new ApiResponse<T>(false, message, default, statusCode);
        }

        public static ApiResponse<T> CreateEmailNotConfirmedResponse<T>(string message, T data)
        {
            return new ApiResponse<T>(true, message, data, StatusCodes.Status403Forbidden);
        }
    }
}
