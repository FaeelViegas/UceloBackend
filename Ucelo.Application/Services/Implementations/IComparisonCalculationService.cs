using Ucelo.Application.DTOs.Calculations;

namespace Ucelo.Application.Services.Interfaces
{
    public interface IComparisonCalculationService
    {
        Task<ComparisonCalculationResponse> CalculateAsync(ComparisonCalculationRequest request);
        Task<ComparisonCalculationResponse> SaveCalculationAsync(int userId, ComparisonCalculationRequest request);
        Task<IEnumerable<ComparisonCalculationResponse>> GetUserCalculationsAsync(int userId);
        Task<ComparisonCalculationResponse> GetByIdAsync(int id);
    }
}