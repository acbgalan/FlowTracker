using Azure;
using FlowTracker.Data.Entities;
using FlowTracker.Server.Services.User;
using FlowTracker.Shared.Dtos.User;
using FluentValidation;
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
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IValidator<UserLoginRequest> _userLoginRequestValidator;

        public UserController(
                    UserManager<User> userManager,
                    SignInManager<User> signInManager,
                    IUserService userService,
                    IConfiguration configuration,
                    IValidator<UserLoginRequest> userLoginRequestValidator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _configuration = configuration;
            _userLoginRequestValidator = userLoginRequestValidator;
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
            // 1. Fast validations
            if (userLoginRequest == null)
            {
                return BadRequest();
            }

            var validationResult = _userLoginRequestValidator.Validate(userLoginRequest);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            // 2. Call service (business logic)
            var serviceResult = await _userService.LoginAsync(userLoginRequest);

            if (!serviceResult.Success)
            {
                return StatusCode(serviceResult.StatusCode, serviceResult.Message);
            }

            return Ok(serviceResult.Data);
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
