using Azure;
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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public UserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<ActionResult<UserAuthenticationResponse>> Register(UserCredentialsRequest userCredentialsRequest)
        {
            var user = new IdentityUser()
            {
                UserName = userCredentialsRequest.Email,
                Email = userCredentialsRequest.Email
            };

            var identityResult = await _userManager.CreateAsync(user, userCredentialsRequest.Password);

            if (identityResult.Succeeded)
            {
                var authenticationResponse = await BuildToken(userCredentialsRequest);
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
        public async Task<ActionResult<UserAuthenticationResponse>> Login(UserCredentialsRequest userCredentialsRequest)
        {
            var user = await _userManager.FindByEmailAsync(userCredentialsRequest.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login");
                return ValidationProblem();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, userCredentialsRequest.Password, false);

            if (result.Succeeded)
            {
                return await BuildToken(userCredentialsRequest);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login");
                return ValidationProblem();
            }
        }


        private async Task<UserAuthenticationResponse> BuildToken(UserCredentialsRequest userCredentials)
        {
            // Create a claim. Information about the user.
            var claims = new List<Claim>
            {
                new Claim("email", userCredentials.Email)
            };

            //We look up the user and retrieve their claims from the database.
            var user = await _userManager.FindByEmailAsync(userCredentials.Email);
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
