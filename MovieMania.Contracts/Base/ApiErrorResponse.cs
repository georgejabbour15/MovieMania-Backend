using Microsoft.AspNetCore.Http;
using System.Collections.Generic;


namespace MovieMania.Contracts.Base
{

    public class ApiErrorResponse : ApiBaseResponse
    {
        public Dictionary<string,string> Errors { get; }
        public string ErrorMessage { get; set; } 

        public ApiErrorResponse(string errorMessage=null, int statusCode=StatusCodes.Status400BadRequest) : base(statusCode)
        {
            Errors = new Dictionary<string, string>();
            ErrorMessage = errorMessage;
        }

        public void AddError(string key, string value) {
            Errors.Add(key, value);
        }
    }

}
