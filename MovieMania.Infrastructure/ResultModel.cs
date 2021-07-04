using Microsoft.AspNetCore.Http;

namespace MovieMania.Infrastructure
{
    public class ResultModel<T>
    {
        public ResultModel(T data = default, bool success = true, int statusCode = StatusCodes.Status200OK, string message = null)
        {
            Success = success;
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }
        public T Data { get; set; }
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }

    }
}
