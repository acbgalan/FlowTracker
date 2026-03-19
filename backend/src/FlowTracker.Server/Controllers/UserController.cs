using Azure;
using FlowTracker.Data.Entities;
using FlowTracker.Shared.Dtos.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FlowTracker.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<ActionResult<UserAuthenticationResponse>> Register(UserRegisterRequest userRegisterRequest)
        {
            var user = new User()
            {
                FirstName = userRegisterRequest.FirstName,
                LastName = userRegisterRequest.LastName,
                UserName = userRegisterRequest.Email,
                Email = userRegisterRequest.Email
            };

            var identityResult = await _userManager.CreateAsync(user, userRegisterRequest.Password);

            if (identityResult.Succeeded)
            {
                var authenticationResponse = await BuildToken(userRegisterRequest.Email);
                return authenticationResponse;
            }
            else
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return ValidationProblem();
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserAuthenticationResponse>> Login(UserLoginRequest userLoginRequest)
        {
            var user = await _userManager.FindByEmailAsync(userLoginRequest.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login");
                return ValidationProblem();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, userLoginRequest.Password, false);

            if (result.Succeeded)
            {
                return await BuildToken(userLoginRequest.Email);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login");
                return ValidationProblem();
            }
        }


        private async Task<UserAuthenticationResponse> BuildToken(string email)
        {
            // Create a claim. Information about the user.
            var claims = new List<Claim>
            {
                new Claim("email",email)
            };

            //We look up the user and retrieve their claims from the database.
            var user = await _userManager.FindByEmailAsync(email);
            var claimsDb = await _userManager.GetClaimsAsync(user!);
            claims.AddRange(claimsDb);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt_key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddMonths(1);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration, signingCredentials: credentials);
            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return new UserAuthenticationResponse
            {
                Token = token,
                Expiration = expiration
            };
        }

    }
}
