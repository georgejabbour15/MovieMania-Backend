using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieMania.Contracts.AuthService.Authenticate;
using MovieMania.Contracts.AuthService.Login;
using MovieMania.Contracts.AuthService.RefreshToken;
using MovieMania.Infrastructure;

namespace AuthService
{
    public interface IAuthService
    {   
        Task<ResultModel<bool>> Register(AuthenticateRequest model);
        Task<ResultModel<IActionResult>> Logout();
        Task<ResultModel<LoginResponse>> Login(LoginRequest model);
        Task<ResultModel<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest model);
    }    
}
