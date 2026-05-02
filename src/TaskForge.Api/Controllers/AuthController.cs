using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskForge.Api.Dto;
using TaskForge.Identity.Jwt;
using TaskForge.Identity.Models;
using TaskForge.Identity.Users;

namespace TaskForge.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly JwtSettings _jwSettings;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService, 
            JwtSettings jwtSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _jwSettings = jwtSettings;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser
            {
                UserName = registerRequest.Email,
                Email = registerRequest.Email, 
                FirstName= "Unknown", 
                LastName= "User"
            };

            var result = await _userManager.CreateAsync(user,
                registerRequest.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _userManager.AddToRoleAsync(user, "User");

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(loginRequest.Email);

            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, false);

            if (result.Succeeded)
            {
                var token = await _tokenService.GenerateTokenAsync(user);

                var roles = await _userManager.GetRolesAsync(user);

                return Ok(new AuthResponse
                {
                    AccessToken = token,
                    ExpiresIn = _jwSettings.ExpiryMinutes * 60,
                    Email = user.Email,
                    Roles = roles
                });
            }

            return Unauthorized("Invalid email or password");
        }
    }
}
