using Ucelo.Application.DTOs.Individuals;
using Ucelo.Application.Services.Interfaces;
using Ucelo.Domain.Entities;
using Ucelo.Domain.Interfaces;

namespace Ucelo.Application.Services.Implementations;

public class IndividualService : IIndividualService
{
    private readonly IIndividualRepository _individualRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public IndividualService(
        IIndividualRepository individualRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _individualRepository = individualRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IndividualResponse> CreateAsync(int userId, CreateIndividualRequest request)
    {
        await ValidateUserExistsAsync(userId);
        await ValidateTaxIdIsUniqueAsync(request.TaxId);
        await ValidateUserDoesNotHaveIndividualAsync(userId);

        var individual = new Individual(
            userId,
            request.FullName,
            request.TaxId,
            request.IdentityCard,
            request.BirthDate,
            request.Phone,
            request.MobilePhone);

        await _individualRepository.AddAsync(individual);
        await _unitOfWork.CommitAsync();

        return MapToResponse(individual);
    }

    public async Task<IndividualResponse> GetByIdAsync(int id)
    {
        var individual = await _individualRepository.GetByIdAsync(id);

        if (individual == null)
            throw new KeyNotFoundException($"Pessoa física com ID {id} não encontrada");

        return MapToResponse(individual);
    }

    public async Task<IndividualResponse?> GetByUserIdAsync(int userId)
    {
        var individual = await _individualRepository.GetByUserIdAsync(userId);
        return individual == null ? null : MapToResponse(individual);
    }

    public async Task<IndividualResponse?> GetByTaxIdAsync(string taxId)
    {
        var individual = await _individualRepository.GetByTaxIdAsync(taxId);
        return individual == null ? null : MapToResponse(individual);
    }

    public async Task<IEnumerable<IndividualResponse>> GetAllAsync()
    {
        var individuals = await _individualRepository.GetAllAsync();
        return individuals.Select(MapToResponse);
    }

    public async Task<IndividualResponse> UpdateAsync(int id, CreateIndividualRequest request)
    {
        var individual = await _individualRepository.GetByIdAsync(id);

        if (individual == null)
            throw new KeyNotFoundException($"Pessoa física com ID {id} não encontrada");

        individual.Update(
            request.FullName,
            request.IdentityCard,
            request.BirthDate,
            request.Phone,
            request.MobilePhone);

        await _individualRepository.UpdateAsync(individual);
        await _unitOfWork.CommitAsync();

        return MapToResponse(individual);
    }

    private async Task ValidateUserExistsAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new KeyNotFoundException($"Usuário com ID {userId} não encontrado");
    }

    private async Task ValidateTaxIdIsUniqueAsync(string taxId)
    {
        if (await _individualRepository.TaxIdExistsAsync(taxId))
            throw new InvalidOperationException("CPF já cadastrado");
    }

    private async Task ValidateUserDoesNotHaveIndividualAsync(int userId)
    {
        var existing = await _individualRepository.GetByUserIdAsync(userId);
        if (existing != null)
            throw new InvalidOperationException("Usuário já possui um perfil de pessoa física");
    }

    private IndividualResponse MapToResponse(Individual individual)
    {
        return new IndividualResponse
        {
            Id = individual.Id,
            UserId = individual.UserId,
            FullName = individual.FullName,
            TaxId = individual.TaxId,
            IdentityCard = individual.IdentityCard,
            BirthDate = individual.BirthDate,
            Phone = individual.Phone,
            MobilePhone = individual.MobilePhone,
            CreatedAt = individual.CreatedAt
        };
    }
}