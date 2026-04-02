using AutoMapper;
using FlowTracker.Server.Services.Common;
using FlowTracker.Shared.Dtos.Common;
using FlowTracker.Shared.Dtos.User;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AppUser = FlowTracker.Data.Entities.User;

namespace FlowTracker.Server.Services.User
{
    public class UserService : BaseService, IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public UserService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IConfiguration configuration,
            IHttpContextAccessor httpContext,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _httpContext = httpContext;
            _mapper = mapper;
        }

        public async Task<ServiceResult<UserAuthenticationResponse>> RegisterAsync(UserRegisterRequest userRegisterRequest)
        {
            var user = _mapper.Map<AppUser>(userRegisterRequest);
            var identityResult = await _userManager.CreateAsync(user, userRegisterRequest.Password);

            if (!identityResult.Succeeded)
            {
                var errorMessage = string.Join("; ", identityResult.Errors.Select(x => x.Description));
                return FailureResult<UserAuthenticationResponse>(errorMessage, StatusCodes.Status400BadRequest);
            }

            var userAuthenticationResponse = await BuildToken(userRegisterRequest.Email);
            return SuccessResult<UserAuthenticationResponse>("", StatusCodes.Status200OK, userAuthenticationResponse);
        }

        public async Task<ServiceResult<UserAuthenticationResponse>> LoginAsync(UserLoginRequest userLoginRequest)
        {
            var user = await _userManager.FindByEmailAsync(userLoginRequest.Email);

            if (user == null)
            {
                return FailureResult<UserAuthenticationResponse>("Invalid login", StatusCodes.Status401Unauthorized);
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, userLoginRequest.Password, false);

            if (!result.Succeeded)
            {
                return FailureResult<UserAuthenticationResponse>("Invalid login", StatusCodes.Status401Unauthorized);
            }

            var userAuthenticationResponse = await BuildToken(userLoginRequest.Email);
            return SuccessResult<UserAuthenticationResponse>("Login successful", StatusCodes.Status200OK, userAuthenticationResponse);
        }


        private async Task<AppUser?> GetUserAsync()
        {
            var emailClaim = _httpContext.HttpContext!.User.Claims.Where(x => x.Type == "email").FirstOrDefault();

            if (emailClaim == null)
            {
                return null;
            }

            var email = emailClaim.Value;
            return await _userManager.FindByEmailAsync(email);
        }

        private async Task<bool> SetAdministrator(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return false;
            }

            await _userManager.AddClaimAsync(user, new Claim("Administrator", "true"));
            return true;
        }

        private async Task<bool> RemoveAdministrator(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return false;
            }

            await _userManager.RemoveClaimAsync(user, new Claim("Administrator", "true"));
            return true;
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

        private bool IsAdministrator()
        {
            var administratorClaim = _httpContext.HttpContext!.User.Claims.Where(x => x.Type == "Administrator").FirstOrDefault();
            return administratorClaim != null ? true : false;
        }

    }
}