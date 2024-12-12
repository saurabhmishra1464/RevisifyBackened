namespace revisifyBackened
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public T Data { get; set; }

    public ApiResponse() 
    {
        IsSuccess = false;
        Message = string.Empty;
        StatusCode = 500;
    }

    public ApiResponse(T data, int statusCode = 200)
    {
            IsSuccess = true;
            Data = data;
            Message = string.Empty;
            StatusCode = statusCode;

    }

        public ApiResponse(string errorMessage, int statusCode)
        {
            IsSuccess = false;
            Data = default;
            Message = errorMessage;
            StatusCode = statusCode;
        }

    }
}
