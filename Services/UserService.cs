using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using StudentManagment.Data;
using StudentManagment.DTOs.Users;
using StudentManagment.Enums;
using StudentManagment.Models;
using StudentManagment.Services.Interfaces;

namespace StudentManagment.Services
{
    public class UserService(SchoolContext context, ITokenService tokenService) : IUserService
    {
        private readonly SchoolContext _context = context;
        private readonly ITokenService _tokenService = tokenService;


        // --- AUTHENTICATION ---

        public async Task<ReadUserDto> RegisterAsync(RegisterUserDto dto)
        {
            // 1. Check for existing email/username
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            {
                throw new InvalidOperationException("Email is already registered.");
            }
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
            {
                throw new InvalidOperationException("Username is already taken.");
            }
            if (dto.Username == null)
            {
                throw new InvalidOperationException("Username cannot be null.");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Username = dto.Username,
                Gender = dto.Gender,
                DateOfBirth = dto.DateOfBirth,
                Phone = dto.Phone,
                PasswordHash = passwordHash,
                Role = UserRoleEnum.Guest 
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new ReadUserDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.Username,
                Role = user.Role,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                Phone = user.Phone
            };
        }

        public async Task<AuthResult> LoginAsync(LoginUserDto dto)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.Email == dto.EmailOrUsername || u.Username == dto.EmailOrUsername) ?? throw new ArgumentException("Invalid credentials ");
            if (!user.IsActive)
            {
                throw new ArgumentException("User account is deactivated.");
            }
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash); 

            if (!isPasswordValid)
            {
                throw new ArgumentException("Invalid credentials.");
            }

            // 2. Generate Tokens
            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            // 3. Update User Refresh Token in DB
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(15);
            user.LastLogin = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // 4. Return Auth Result
            return new AuthResult
            {
                Token = accessToken,
                Expiration = DateTime.UtcNow.AddMinutes(30),
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthResult> RefreshTokenAsync(string refreshToken)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.RefreshToken == refreshToken && u.RefreshTokenExpiryTime > DateTime.UtcNow);

            if (user == null || !user.IsActive)
            {
                throw new ArgumentException("Invalid or expired refresh token.");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(15);

            await _context.SaveChangesAsync();

            return new AuthResult
            {
                Token = newAccessToken,
                Expiration = DateTime.UtcNow.AddMinutes(30),
                RefreshToken = newRefreshToken
            };
        }


        public async Task<ReadUserDto?> GetUserByIdAsync(int userId)
        {
            var user = await _context.Users
                .Include(u => u.Student)
                .Include(u => u.Teacher)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null) return null;

            return new ReadUserDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.Username,
                Phone = user.Phone,
                Role = user.Role,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                StudentId = user.Student?.StudentId,
                TeacherId = user.Teacher?.TeacherId
            };
        }

        public async Task<IEnumerable<ReadUserDto>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Student)
                .Include(u => u.Teacher)
                .Select(user => new ReadUserDto
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Username = user.Username,
                    Phone = user.Phone,
                    Role = user.Role,
                    Gender = user.Gender,
                    DateOfBirth = user.DateOfBirth,
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt,
                    StudentId = user.Student == null ? (int?)null : user.Student.StudentId,
                    TeacherId = user.Teacher == null ? (int?)null : user.Teacher.TeacherId
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateUserProfileAsync(int userId, UpdateUserDto dto)
        {
            var userToUpdate = await _context.Users.FindAsync(userId);
            if (userToUpdate == null) return false;

            if (await _context.Users.AnyAsync(u => u.Username == dto.Username && u.UserId != userId))
            {
                throw new InvalidOperationException("The requested username is already taken by another user.");
            }

            if (dto.Username == null )
            {
                throw new InvalidOperationException("Username cannot be null.");
            }

            userToUpdate.FirstName = dto.FirstName;
            userToUpdate.LastName = dto.LastName;
            userToUpdate.Gender = dto.Gender;
            userToUpdate.DateOfBirth = dto.DateOfBirth;
            userToUpdate.Phone = dto.Phone;
            userToUpdate.Username = dto.Username;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUserPasswordAsync(int userId, UpdatePasswordDto dto, bool isAdminOverride)
        {
            var userToUpdate = await _context.Users.FindAsync(userId);
            if (userToUpdate == null) return false;

            if (!isAdminOverride)
            {
                var isCurrentPasswordValid = BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, userToUpdate.PasswordHash);

                if (!isCurrentPasswordValid)
                {
                    throw new InvalidOperationException("The current password provided is incorrect.");
                }
            }

            userToUpdate.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            await _context.SaveChangesAsync();
            return true;
        }
            

        public async Task<bool> DeactivateUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.IsActive = false;

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            await _context.SaveChangesAsync();
            return true;
        }

        // --- ADMINISTRATIVE ACTIONS ---

        public async Task<bool> UpdateUserRoleAsync(int userId, UpdateUserRoleDto dto)
        {
            var userToUpdate = await _context.Users.FindAsync(userId);
            if (userToUpdate == null) return false;

            if (!userToUpdate.IsActive)
            {
                throw new InvalidOperationException("Cannot change role of a deactivated user.");
            }

            userToUpdate.Role = dto.NewRole;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUserStatusAsync(int userId, UpdateUserStatusDto dto)
        {
            var userToUpdate = await _context.Users.FindAsync(userId);
            if (userToUpdate == null) return false;

            userToUpdate.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}