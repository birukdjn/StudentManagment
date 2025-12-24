using StudentManagment.Models;
using System.Security.Claims;

namespace StudentManagment.Services.Interfaces
{
    // ITokenService.cs
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}