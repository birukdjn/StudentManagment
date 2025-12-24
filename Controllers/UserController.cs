using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagment.DTOs.Users;
using StudentManagment.Services.Interfaces;
using System.Security.Claims; 

namespace StudentManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize] 
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        private int? GetAuthenticatedUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            return null;
        }

        // --- AUTHENTICATION ENDPOINTS (NO AUTH REQUIRED) ---

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<ReadUserDto>> Register(RegisterUserDto dto)
        {
            try
            {
                var readUserDto = await _userService.RegisterAsync(dto);
                return CreatedAtAction(nameof(GetUser), new { id = readUserDto.UserId }, readUserDto);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message); 
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); 
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthResult>> Login(LoginUserDto dto)
        {
            try
            {
                var authResult = await _userService.LoginAsync(dto);
                return Ok(authResult); 
            }
            catch (ArgumentException ex)
            {
                return Unauthorized(ex.Message); 
            }
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResult>> RefreshToken([FromBody] string refreshToken)
        {
            try
            {
                var authResult = await _userService.RefreshTokenAsync(refreshToken);
                return Ok(authResult); 
            }
            catch (ArgumentException ex)
            {
                return Unauthorized(ex.Message); 
            }
        }

        // --- GENERAL READ ENDPOINTS (AUTHENTICATED ONLY) ---

        [HttpGet]
        //[Authorize(Roles = "Admin")] 
        public async Task<ActionResult<IEnumerable<ReadUserDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users); 
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReadUserDto>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound(); 
            }

            return Ok(user); 
        }


        // --- SELF-SERVICE UPDATE ENDPOINTS (Self or Admin Override) ---

        [HttpPut("{id}/profile")]
        public async Task<IActionResult> UpdateProfile(int id, UpdateUserDto dto)
        {
            var currentUserId = GetAuthenticatedUserId();

            // Self-service OR Admin check
            if (currentUserId != id && !User.IsInRole("Admin"))
            {
                return Forbid("You can only update your own profile."); 
            }

            try
            {
                var success = await _userService.UpdateUserProfileAsync(id, dto);

                if (!success)
                {
                    return NotFound(); 
                }

                return NoContent(); 
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message); 
            }
        }


        [HttpPut("{id}/password")]
        public async Task<IActionResult> UpdatePassword(int id, UpdatePasswordDto dto)
        {
            var currentUserId = GetAuthenticatedUserId();
            var isAdmin = User.IsInRole("Admin");

            // Self-service OR Admin check
            if (currentUserId != id && !isAdmin)
            {
                return Forbid("You can only change your own password."); 
            }


            try
            {
                var success = await _userService.UpdateUserPasswordAsync(id, dto, isAdmin);

                if (!success)
                {
                    return NotFound(); 
                }

                return NoContent(); 
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(ex.Message); 
            }
        }

        // --- ADMINISTRATIVE/DEACTIVATION ENDPOINTS (Admin Only or Self) ---

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeactivateUser(int id)
        {
            var currentUserId = GetAuthenticatedUserId();

            // Self-service OR Admin check
            if (currentUserId != id && !User.IsInRole("Admin"))
            {
                return Forbid("You are not authorized to deactivate this account."); 
            }

            var success = await _userService.DeactivateUserAsync(id);

            if (!success)
            {
                return NotFound(); 
            }

            return NoContent(); 
        }


        [HttpPut("{id}/role")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> UpdateRole(int id, UpdateUserRoleDto dto)
        {
            if (id != dto.UserId)
            {
                return BadRequest("User ID mismatch.");
            }

            try
            {
                var success = await _userService.UpdateUserRoleAsync(id, dto);

                if (!success)
                {
                    return NotFound();
                }

                return NoContent(); 
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); 
            }
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> UpdateStatus(int id, UpdateUserStatusDto dto)
        {
            if (id != dto.UserId)
            {
                return BadRequest("User ID mismatch.");
            }

            var success = await _userService.UpdateUserStatusAsync(id, dto);

            if (!success)
            {
                return NotFound(); 
            }

            return NoContent(); 
        }
    }
}