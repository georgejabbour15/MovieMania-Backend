using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using MovieMania.Data.DataContext;


namespace MovieMania.AuthHandler
{

    public class CustomAuthHandler : AuthenticationHandler<CustomAuthOptions>
    {
 
        private readonly ApplicationDbContext _context;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public CustomAuthHandler(
            IOptionsMonitor<CustomAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, ApplicationDbContext context, TokenValidationParameters tokenValidationParameters) : base(options, logger, encoder, clock)
        {

            _tokenValidationParameters = tokenValidationParameters;
            _context = context;
        }

      
        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // validation comes in here

            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return await Task.FromResult(AuthenticateResult.Fail("Header Not Found."));
            }

            string token = Request.Headers["Authorization"];
            token = token["bearer ".Length..];

            if (string.IsNullOrEmpty(token))
            {
                return await Task.FromResult(AuthenticateResult.Fail("Unauthorized."));
            }
            if (!await IsValidToken(token))
            {
                return await Task.FromResult(AuthenticateResult.Fail("Unauthorized."));
            }
            ClaimsPrincipal principal = GetPrincipalFromToken(token);
            var ticket = new AuthenticationTicket(principal, CustomAuthConstants.UserAuthScheme);

            return await Task.FromResult(AuthenticateResult.Success(ticket));
        }
        private async Task<bool> IsValidToken(string token)
        {
            ClaimsPrincipal validatedToken = GetPrincipalFromToken(token);
            if (validatedToken is null)
                return false;
            //check expiry
            long tokenExpiryTimeStamp = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
            DateTime expiryToken = new DateTime(1970, 1, 1, 0, 0, 0,DateTimeKind.Utc).AddSeconds(tokenExpiryTimeStamp);
            if (expiryToken > DateTime.UtcNow)
            {
                string userId = validatedToken.Claims.First().Value;
                if (userId is null)
                    return false;
                bool user = await _context.Users.AnyAsync(user => user.Id == userId);
                if (!user)
                    return false;

                bool refreshTokenExists= await _context.RefreshToken.AnyAsync(rt=>rt.User.Id==userId);
                if(refreshTokenExists is false)
                    return false;
                
                return true;
            }
            return false;
        }

        private static bool IsValidJWTSecurityAlgorithm(SecurityToken securityToken)
        {
            //this token is of type jwtsecuritytoken && check algo
            return (securityToken is JwtSecurityToken jwtSecurityToken) && (jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase));
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            try
            {
                JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                var tokenValidationParameters = _tokenValidationParameters.Clone();
                tokenValidationParameters.ValidateLifetime = false;
                ClaimsPrincipal claimsPrincipal = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                if (!IsValidJWTSecurityAlgorithm(validatedToken))
                    return null;
                return claimsPrincipal;
            }
            catch { return null; }
        }
    }
}


