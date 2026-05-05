using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] AuthRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _authService.ValidateUserCredentialsAsync(request.Email, request.Password);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            var token = _authService.CreateToken(user);
            return Ok(new AuthResponseDto
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60),
                Role = user.Role,
                UserId = user.Id
            });
        }

        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> SignUp([FromBody] SignUpRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _userService.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return Conflict(new { message = "Email is already registered." });
            }

            var createdUser = await _userService.CreateUserAsync(new UserCreateDto
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password,
                Role = "User"
            });

            var userEntity = await _userService.FindByEmailAsync(request.Email);
            if (userEntity == null)
            {
                return StatusCode(500, new { message = "Failed to create user." });
            }

            var token = _authService.CreateToken(userEntity);
            return Ok(new AuthResponseDto
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60),
                Role = userEntity.Role,
                UserId = userEntity.Id
            });
        }
    }
}
