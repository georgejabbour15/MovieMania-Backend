

namespace MovieMania.Contracts.Base
{
    public class ApiBaseResponse
    {
        public ApiBaseResponse(int statusCode)
        {
            StatusCode = statusCode;
        }

        public int StatusCode { get; }

    }

}
