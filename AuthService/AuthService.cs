
using System;
using System.Linq;
using System.Text;
using CurrentUserService;
using MovieMania.Infrastructure;
using System.Threading.Tasks;
using System.Security.Claims;
using MovieMania.Data.DataContext;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MovieMania.Infrastructure.Domains;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using MovieMania.Contracts.AuthService.RefreshToken;
using Microsoft.AspNetCore.Mvc;
using MovieMania.Contracts.AuthService.Authenticate;
using MovieMania.Contracts.Enums;
using MovieMania.Contracts.AuthService.Login;

namespace AuthService
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<User> _userManager;
        public AuthService(UserManager<User> userManager,
            IConfiguration configuration,
            ApplicationDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
            _currentUserService = currentUserService;
        }

        public async Task<ResultModel<bool>> Register(AuthenticateRequest model)
        {
            User user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                user = new User()
                {
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    Email = model.Email,
                    UserName=model.Email,
                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                    return new ResultModel<bool>(false, false, StatusCodes.Status500InternalServerError, "User creation failed");

                await _userManager.AddToRoleAsync(user, UserRolesEnums.User);
            }

            return new ResultModel<bool>(true, true, message: "User created successfully");
        }

        public async Task<ResultModel<LoginResponse>> Login(LoginRequest model)
        {

            User user = await _userManager.FindByEmailAsync(model.Email);

            LoginResponse loginResponse = new LoginResponse();

            if (user is null)
                return new ResultModel<LoginResponse>(null, false, StatusCodes.Status404NotFound, "User doesn't exist");

            loginResponse.User = user;

            var passwordOK = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordOK)
                return new ResultModel<LoginResponse>(null, false, StatusCodes.Status401Unauthorized, "Incorrect password");

            IList<string> userRoles = await _userManager.GetRolesAsync(user);

            loginResponse.Token = new JwtSecurityTokenHandler().WriteToken(GenerateJwt(user, userRoles));

            RefreshToken activeRefreshToken = _context.RefreshToken.Where(t => t.UserId == user.Id && t.Expires > DateTime.UtcNow).FirstOrDefault();

            if (activeRefreshToken is not null)
            {
                loginResponse.RefreshToken = activeRefreshToken.Id;
                return new ResultModel<LoginResponse>(loginResponse, true, message: "Login Successfull");
            }
          
            else
            {
                var refreshToken = CreateRefreshToken();
                refreshToken.UserId = user.Id;
                loginResponse.RefreshToken = refreshToken.Id;
                _context.RefreshToken.Add(refreshToken);
                var result = await _context.SaveChangesAsync();
                if (result < 0)
                    return new ResultModel<LoginResponse>(null, false, StatusCodes.Status500InternalServerError, "Internal server error");

                return new ResultModel<LoginResponse>(loginResponse, true, message: "Login Successfull");
            }
        }

        public async Task<ResultModel<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
        {
        
            var refreshTokenResponse = new RefreshTokenResponse();

            string userId = _currentUserService.GetUserId(request.Token);

            User user = _context.Users.Where(user => user.Id == userId).FirstOrDefault();


            if (user == null)
            {
                return new ResultModel<RefreshTokenResponse>(null, false, StatusCodes.Status400BadRequest, "Token did not match any users.");
            }

            RefreshToken dbRefToken = _context.RefreshToken.Single(t => t.Id == request.RefreshToken);

            if (dbRefToken.UserId != user.Id || dbRefToken.Id != request.RefreshToken)
            {
                return new ResultModel<RefreshTokenResponse>(null, false, StatusCodes.Status400BadRequest, "Unauthorized access.");
            }

            if (dbRefToken.Expires < DateTime.UtcNow && dbRefToken.KeepLogin == false)
            { //expired
                return new ResultModel<RefreshTokenResponse>(null, false, StatusCodes.Status400BadRequest, "Session has ended. Please login again");
            }

            //Generates new jwt
            var userRoles = await _userManager.GetRolesAsync(user);
            JwtSecurityToken newToken = GenerateJwt(user, userRoles);
            refreshTokenResponse.Token = new JwtSecurityTokenHandler().WriteToken(newToken);
            refreshTokenResponse.RefreshToken = request.RefreshToken;
            refreshTokenResponse.User = user;

            return new ResultModel<RefreshTokenResponse>(refreshTokenResponse, true);
        }

        private JwtSecurityToken GenerateJwt(User user, IList<string> userRoles)
        {
            List<Claim> authClaims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            foreach (var userrole in userRoles)
                authClaims.Add(new Claim(ClaimTypes.Role, userrole));//ClaimTypes = asp.net core

            SymmetricSecurityKey authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            JwtSecurityToken newToken = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],//gives the token
                audience: _configuration["JWT:ValidAudience"],//receives the token
                expires: DateTime.Now.AddMinutes(int.Parse(_configuration["JWT:DurationInMinutes"])),
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256),
                claims: authClaims
                );

            return newToken;
        }

        private static RefreshToken CreateRefreshToken()
        {
            return new RefreshToken
            {
                Id = Guid.NewGuid().ToString(),
                Expires = DateTime.UtcNow.AddDays(10),
                Created = DateTime.UtcNow
            };
        }

        public async Task<ResultModel<IActionResult>> Logout()
        {
            var data = await _currentUserService.GetCurrentUser();
            User user = data.Data.User;

            if (user is null)
                return new ResultModel<IActionResult>(null, false, StatusCodes.Status400BadRequest, "User doesn't exist");

            RefreshToken refreshToken = _context.RefreshToken.Single(rt => rt.UserId == user.Id);

            _context.RefreshToken.Remove(refreshToken);
            var result = await _context.SaveChangesAsync();

            if (result < 0)
                return new ResultModel<IActionResult>(null, false, StatusCodes.Status500InternalServerError, "Internal server error");

            return new ResultModel<IActionResult>(null, true, message: "Logout Successfull");

        }


    }
}
