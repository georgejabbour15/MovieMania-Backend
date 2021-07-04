using Microsoft.AspNetCore.Http;


namespace MovieMania.Contracts.Base
{
    public class ApiOkResponse : ApiBaseResponse
    {
        public object Data { get; }
        public ApiOkResponse(object data=null) : base(StatusCodes.Status200OK)
        {
            Data = data;
        }
    }
}
