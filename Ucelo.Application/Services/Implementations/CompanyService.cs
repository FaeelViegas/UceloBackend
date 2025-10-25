using Ucelo.Application.DTOs.Companies;
using Ucelo.Application.Services.Interfaces;
using Ucelo.Domain.Entities;
using Ucelo.Domain.Interfaces;

namespace Ucelo.Application.Services.Implementations;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CompanyService(
        ICompanyRepository companyRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _companyRepository = companyRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CompanyResponse> CreateAsync(int userId, CreateCompanyRequest request)
    {
        await ValidateUserExistsAsync(userId);
        await ValidateTaxIdIsUniqueAsync(request.TaxId);
        await ValidateUserDoesNotHaveCompanyAsync(userId);

        var company = new Company(
            userId,
            request.LegalName,
            request.TaxId,
            request.TradeName,
            request.StateRegistration,
            request.Phone,
            request.CorporateEmail);

        await _companyRepository.AddAsync(company);
        await _unitOfWork.CommitAsync();

        return MapToResponse(company);
    }

    public async Task<CompanyResponse> GetByIdAsync(int id)
    {
        var company = await _companyRepository.GetByIdAsync(id);

        if (company == null)
            throw new KeyNotFoundException($"Empresa com ID {id} não encontrada");

        return MapToResponse(company);
    }

    public async Task<CompanyResponse?> GetByUserIdAsync(int userId)
    {
        var company = await _companyRepository.GetByUserIdAsync(userId);
        return company == null ? null : MapToResponse(company);
    }

    public async Task<CompanyResponse?> GetByTaxIdAsync(string taxId)
    {
        var company = await _companyRepository.GetByTaxIdAsync(taxId);
        return company == null ? null : MapToResponse(company);
    }

    public async Task<IEnumerable<CompanyResponse>> GetAllAsync()
    {
        var companies = await _companyRepository.GetAllAsync();
        return companies.Select(MapToResponse);
    }

    public async Task<IEnumerable<CompanyResponse>> GetActiveCompaniesAsync()
    {
        var companies = await _companyRepository.GetActiveCompaniesAsync();
        return companies.Select(MapToResponse);
    }

    public async Task<CompanyResponse> UpdateAsync(int id, CreateCompanyRequest request)
    {
        var company = await _companyRepository.GetByIdAsync(id);

        if (company == null)
            throw new KeyNotFoundException($"Empresa com ID {id} não encontrada");

        company.Update(
            request.LegalName,
            request.TradeName,
            request.StateRegistration,
            request.Phone,
            request.CorporateEmail);

        await _companyRepository.UpdateAsync(company);
        await _unitOfWork.CommitAsync();

        return MapToResponse(company);
    }

    public async Task DeactivateAsync(int id)
    {
        var company = await _companyRepository.GetByIdAsync(id);

        if (company == null)
            throw new KeyNotFoundException($"Empresa com ID {id} não encontrada");

        company.Deactivate();
        await _companyRepository.UpdateAsync(company);
        await _unitOfWork.CommitAsync();
    }

    public async Task ActivateAsync(int id)
    {
        var company = await _companyRepository.GetByIdAsync(id);

        if (company == null)
            throw new KeyNotFoundException($"Empresa com ID {id} não encontrada");

        company.Activate();
        await _companyRepository.UpdateAsync(company);
        await _unitOfWork.CommitAsync();
    }

    private async Task ValidateUserExistsAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new KeyNotFoundException($"Usuário com ID {userId} não encontrado");
    }

    private async Task ValidateTaxIdIsUniqueAsync(string taxId)
    {
        if (await _companyRepository.TaxIdExistsAsync(taxId))
            throw new InvalidOperationException("CNPJ já cadastrado");
    }

    private async Task ValidateUserDoesNotHaveCompanyAsync(int userId)
    {
        var existing = await _companyRepository.GetByUserIdAsync(userId);
        if (existing != null)
            throw new InvalidOperationException("Usuário já possui um perfil de empresa");
    }

    private CompanyResponse MapToResponse(Company company)
    {
        return new CompanyResponse
        {
            Id = company.Id,
            UserId = company.UserId,
            LegalName = company.LegalName,
            TradeName = company.TradeName,
            TaxId = company.TaxId,
            StateRegistration = company.StateRegistration,
            Phone = company.Phone,
            CorporateEmail = company.CorporateEmail,
            IsActive = company.IsActive,
            CreatedAt = company.CreatedAt
        };
    }
}