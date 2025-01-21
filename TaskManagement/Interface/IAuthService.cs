using TaskManagement.Models;
using TaskManagement.DTOs;

namespace TaskManagement.Services
{
    public interface IAuthService
    {
        Task<ServiceResult> RegisterAsync(RegisterRequest registerDto);
        Task<string?> LoginAsync(LoginRequest loginDto);
    }
}
