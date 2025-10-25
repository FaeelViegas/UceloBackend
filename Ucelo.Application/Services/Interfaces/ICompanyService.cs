using Ucelo.Application.DTOs.Companies;

namespace Ucelo.Application.Services.Interfaces;

public interface ICompanyService
{
    Task<CompanyResponse> CreateAsync(int userId, CreateCompanyRequest request);
    Task<CompanyResponse> GetByIdAsync(int id);
    Task<CompanyResponse?> GetByUserIdAsync(int userId);
    Task<CompanyResponse?> GetByTaxIdAsync(string taxId);
    Task<IEnumerable<CompanyResponse>> GetAllAsync();
    Task<IEnumerable<CompanyResponse>> GetActiveCompaniesAsync();
    Task<CompanyResponse> UpdateAsync(int id, CreateCompanyRequest request);
    Task DeactivateAsync(int id);
    Task ActivateAsync(int id);
}