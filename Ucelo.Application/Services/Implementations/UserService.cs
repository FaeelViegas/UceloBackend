using Ucelo.Application.DTOs.Users;
using Ucelo.Application.Services.Interfaces;
using Ucelo.Domain.Entities;
using Ucelo.Domain.Enums;
using Ucelo.Domain.Interfaces;
using Ucelo.Infrastructure.Security;

namespace Ucelo.Application.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserResponse> CreateAsync(CreateUserRequest request)
    {
        await ValidateEmailIsUniqueAsync(request.Email);

        var userType = ParseUserType(request.UserType);
        var (passwordHash, saltHash) = _passwordHasher.Hash(request.Password);

        var user = new User(request.Email, passwordHash, saltHash, userType);

        await _userRepository.AddAsync(user);
        await _unitOfWork.CommitAsync();

        return MapToResponse(user);
    }

    public async Task<UserResponse> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
            throw new KeyNotFoundException($"Usuário com ID {id} não encontrado");

        return MapToResponse(user);
    }

    public async Task<UserResponse?> GetByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        return user == null ? null : MapToResponse(user);
    }

    public async Task<IEnumerable<UserResponse>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(MapToResponse);
    }

    public async Task<IEnumerable<UserResponse>> GetActiveUsersAsync()
    {
        var users = await _userRepository.GetActiveUsersAsync();
        return users.Select(MapToResponse);
    }

    private async Task ValidateEmailIsUniqueAsync(string email)
    {
        if (await _userRepository.EmailExistsAsync(email))
            throw new InvalidOperationException("E-mail já cadastrado");
    }

    private UserType ParseUserType(string userTypeString)
    {
        if (!Enum.TryParse<UserType>(userTypeString, true, out var userType))
            throw new ArgumentException($"Tipo de usuário inválido: {userTypeString}");

        return userType;
    }

    private UserResponse MapToResponse(User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            Email = user.Email,
            UserType = user.UserType.ToString(),
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt
        };
    }
}