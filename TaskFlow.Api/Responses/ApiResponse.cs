namespace TaskFlow.Api.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new();

        public ApiResponse(T data, string message = "")
        {
            Success = true;
            Data = data;
            Message = message;
        }

        public ApiResponse(string message, List<string> errors)
        {
            Success = false;
            Message = message;
            Errors = errors;
        }
    }
}

