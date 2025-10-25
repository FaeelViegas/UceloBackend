using Ucelo.Application.DTOs.Individuals;

namespace Ucelo.Application.Services.Interfaces;

public interface IIndividualService
{
    Task<IndividualResponse> CreateAsync(int userId, CreateIndividualRequest request);
    Task<IndividualResponse> GetByIdAsync(int id);
    Task<IndividualResponse?> GetByUserIdAsync(int userId);
    Task<IndividualResponse?> GetByTaxIdAsync(string taxId);
    Task<IEnumerable<IndividualResponse>> GetAllAsync();
    Task<IndividualResponse> UpdateAsync(int id, CreateIndividualRequest request);
}