﻿
namespace MovieMania.Contracts.AuthService.Authenticate
{
    public class AuthenticateRequest
    {
       
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
