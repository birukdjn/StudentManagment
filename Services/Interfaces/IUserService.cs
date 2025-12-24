using StudentManagment.DTOs.Users;
using StudentManagment.Models;
using System.Security.Claims;

namespace StudentManagment.Services.Interfaces
{
    public interface IUserService
    {
        // --- AUTHENTICATION ---

        // Registration: Creates the User record in the database
        Task<ReadUserDto> RegisterAsync(RegisterUserDto dto);

        // Login: Validates credentials and generates a token
        Task<AuthResult> LoginAsync(LoginUserDto dto);

        // Refresh Token: Generates a new access token using a refresh token
        Task<AuthResult> RefreshTokenAsync(string refreshToken);


        // --- CRUD OPERATIONS ---

        // Read
        Task<ReadUserDto?> GetUserByIdAsync(int userId);
        Task<IEnumerable<ReadUserDto>> GetAllUsersAsync();

        // Update (Profile)
        Task<bool> UpdateUserProfileAsync(int userId, UpdateUserDto dto);

        // Update (Password)
        Task<bool> UpdateUserPasswordAsync(int userId, UpdatePasswordDto dto, bool isAdminOverride);

        // Delete (Soft Delete/Deactivate)
        Task<bool> DeactivateUserAsync(int userId);

        // --- ADMINISTRATIVE ACTIONS (Typically Admin-only) ---

        // Update Role
        Task<bool> UpdateUserRoleAsync(int userId, UpdateUserRoleDto dto);

        // Update Active Status
        Task<bool> UpdateUserStatusAsync(int userId, UpdateUserStatusDto dto);
    }

    // A helper class to return authentication results (Token, Refresh Token, etc.)
    // This is often kept separate from DTOs for clarity.
    public class AuthResult
    {
        public required string Token { get; set; }
        public required DateTime Expiration { get; set; }
        public required string RefreshToken { get; set; }
    }
}