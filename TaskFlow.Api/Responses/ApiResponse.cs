using TaskFlow.Core.CustomEntities;

namespace TaskFlow.Api.Responses
{
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public Pagination? Pagination { get; set; }
        public Message[]? Messages { get; set; }
        public bool Success { get; set; }

        // Constructor exito con datos y mensaje
        public ApiResponse(T data, string message = "")
        {
            Success = true;
            Data = data;
            Messages = new[]
            {
                new Message { Type = Core.Enum.TypeMessage.success.ToString(), Description = message }
            };
        }

        // Constructor exito con datos, mensaje y paginacion
        public ApiResponse(T data, string message, Pagination pagination)
        {
            Success = true;
            Data = data;
            Messages = new[]
            {
                new Message { Type = Core.Enum.TypeMessage.success.ToString(), Description = message }
            };
            Pagination = pagination;
        }

        // Constructor error
        public ApiResponse(string message, List<string>? errors = null)
        {
            Success = false;
            Messages = new[]
            {
                new Message { Type = Core.Enum.TypeMessage.error.ToString(), Description = message }
            };
            Data = default;
        }
    }
}
