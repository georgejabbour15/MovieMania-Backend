
using AuthService;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MovieMania.Contracts.Routes;
using MovieMania.Contracts.AuthService.Authenticate;
using MovieMania.Contracts.Base;
using MovieMania.AuthHandler;
using MovieMania.Contracts.AuthService.RefreshToken;
using MovieMania.Contracts.AuthService.Login;

namespace MovieMania.Controllers.V1
{
    [ApiController]
    [ApiVersion("1")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost(ApiRoutes.Auth.Register)]
        public async Task<IActionResult> Register([FromBody] AuthenticateRequest model)
        {
            var result = await _authService.Register(model);
            return result.Success ? Ok(new ApiOkResponse(result.Data)) : BadRequest(new ApiErrorResponse(result.Message));
        }

        [HttpGet(ApiRoutes.Auth.Logout)]
        [Authorize(AuthenticationSchemes = CustomAuthConstants.UserAuthScheme)]
        public async Task<IActionResult> Logout()
        {
            var result = await _authService.Logout();
            return result.Success ? Ok(new ApiOkResponse(result.Data)) : BadRequest(new ApiErrorResponse(result.Message));
        }


        [HttpPost(ApiRoutes.Auth.Login)]
        public async Task<IActionResult> Login ([FromBody] LoginRequest model)
        {
            var result = await _authService.Login(model);
            return result.Success ? Ok(new ApiOkResponse(result.Data)) : BadRequest(new ApiErrorResponse(result.Message));
        }

        [HttpPost(ApiRoutes.Auth.RefreshToken)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest model)
        {
            var result = await _authService.RefreshTokenAsync(model);
            return result.Success ? Ok(new ApiOkResponse(result.Data)) : BadRequest(new ApiErrorResponse(result.Message));
        }
    }
}

