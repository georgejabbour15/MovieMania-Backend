
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Headers;

using System.Collections.Generic;
using MovieMania.Infrastructure.Domains;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Net.Http.Headers;
using MovieMania.Contracts.CurrentUserService.GetCurrentUser;
using MovieMania.Infrastructure;

namespace CurrentUserService
{
    public class CurrentUserService:ICurrentUserService
    {
        private readonly HttpContext _httpContext;
        private readonly UserManager<User> _userManager;
        public CurrentUserService(IHttpContextAccessor httpContext, UserManager<User> userManager)
        {
            _httpContext = httpContext.HttpContext;
            _userManager = userManager;
        }

        public async Task<ResultModel<GetCurrentUserResponse>> GetCurrentUser() {

            GetCurrentUserResponse getCurrentUserResponse = new GetCurrentUserResponse();
            User currentUser= await _userManager.FindByIdAsync(GetUserId());
            if(currentUser is null)
                return new ResultModel<GetCurrentUserResponse>(null, false, StatusCodes.Status400BadRequest, "No current user");
            
            IList<string> userRoles = await _userManager.GetRolesAsync(currentUser);
            getCurrentUserResponse.User = currentUser;
            getCurrentUserResponse.Role = (List<string>)userRoles;

            return new ResultModel<GetCurrentUserResponse>(getCurrentUserResponse, true,message:"This is the current user");
        }

        public string GetUserId()
        {
            JwtSecurityToken decodedToken = DecodeJwt(GetToken());
            string userId = decodedToken.Claims.First(c => c.Type == "sub").Value;
            return userId;
        }

        private string GetToken()
        {
            string token = "";
            var authorization = _httpContext.Request.Headers[HeaderNames.Authorization];
            if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
            {
                token = headerValue.Parameter;
            }
            return token;
        }
       
        //New
        private static JwtSecurityToken DecodeJwt(string token) => new JwtSecurityToken(jwtEncodedString: token);

        public string GetUserId(string token)
        {
            JwtSecurityToken decodedToken = DecodeJwt(token);
            string userId = decodedToken.Claims.First(c => c.Type == "sub").Value;
            return userId;
        }
    }
}
