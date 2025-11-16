using Ucelo.Application.DTOs.Calculations;

namespace Ucelo.Application.Services.Interfaces;

public interface IPowerCalculationService
{
    Task<PowerCalculationResponse> CalculateAsync(PowerCalculationRequest request);
    Task<PowerCalculationResponse> SaveCalculationAsync(int userId, PowerCalculationRequest request);
    Task<IEnumerable<PowerCalculationResponse>> GetUserCalculationsAsync(int userId);
    Task<PowerCalculationResponse?> GetByIdAsync(int id);
}