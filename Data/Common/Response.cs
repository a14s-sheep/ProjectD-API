namespace ProjectD_API.Data.Common
{
    public class Response<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public Response(int statusCode, string message, T data)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }

        // Helper methods for common responses
        public static Response<T> SuccessResponse(T data, string message = "Success")
        {
            return new Response<T>(200, message, data);
        }

        public static Response<T> ErrorResponse(string? message = "An unexpected error occurred.", int statusCode = 400)
        {
            return new Response<T>(statusCode, message ?? "An unexpected error occurred.", default);
        }
    }
}
