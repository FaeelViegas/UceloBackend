using Ucelo.Application.DTOs.Users;

namespace Ucelo.Application.Services.Interfaces;

public interface IUserService
{
    Task<UserResponse> CreateAsync(CreateUserRequest request);
    Task<UserResponse> GetByIdAsync(int id);
    Task<UserResponse?> GetByEmailAsync(string email);
    Task<IEnumerable<UserResponse>> GetAllAsync();
    Task<IEnumerable<UserResponse>> GetActiveUsersAsync();
}