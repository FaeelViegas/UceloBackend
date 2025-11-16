using System.Text.Json;
using Ucelo.Application.DTOs.Calculations;
using Ucelo.Application.Services.Interfaces;
using Ucelo.Domain.Entities;
using Ucelo.Domain.Enums;
using Ucelo.Domain.Interfaces;

namespace Ucelo.Application.Services.Implementations;

public class PowerCalculationService : IPowerCalculationService
{
    private readonly ICalculationRepository _calculationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly JsonSerializerOptions _jsonOptions;

    public PowerCalculationService(
        ICalculationRepository calculationRepository,
        IUnitOfWork unitOfWork)
    {
        _calculationRepository = calculationRepository;
        _unitOfWork = unitOfWork;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
    }

    public async Task<PowerCalculationResponse> CalculateAsync(PowerCalculationRequest request)
    {
        if (request.Height <= 0 || request.Rotation <= 0 || request.Capacity <= 0)
        {
            throw new ArgumentException("Altura, rotação e capacidade devem ser maiores que zero.");
        }

        double powerKW;

        if (request.Efficiency > 0 && request.PowerFactor > 0)
        {
            powerKW = (request.Height * request.Capacity * request.Gravity) /
                      (3600 * request.Efficiency * request.PowerFactor);
        }
        else
        {
            powerKW = (request.Height * request.Capacity * request.Gravity) /
                      (3600 * request.GeneralEfficiency);
        }

        double powerCV = powerKW * 1.36;

        double moment = (9550 * powerKW) / request.Rotation;

        double maxMoment = moment * request.ServiceFactor;

        return new PowerCalculationResponse
        {
            PowerKW = Math.Round(powerKW, 2),
            PowerCV = Math.Round(powerCV, 2),
            Moment = Math.Round(moment, 2),
            MaxMoment = Math.Round(maxMoment, 2)
        };
    }

    public async Task<PowerCalculationResponse> SaveCalculationAsync(int userId, PowerCalculationRequest request)
    {
        var result = await CalculateAsync(request);

        var calculationName = string.IsNullOrWhiteSpace(request.Name)
            ? $"Cálculo de Potência - {DateTime.UtcNow:dd/MM/yyyy HH:mm}"
            : request.Name;

        var inputData = JsonSerializer.Serialize(request, _jsonOptions);
        var resultData = JsonSerializer.Serialize(result, _jsonOptions);

        var calculation = new Calculation(
            userId,
            calculationName,
            CalculationType.Power,
            inputData,
            resultData
        );

        await _calculationRepository.AddAsync(calculation);
        await _unitOfWork.CommitAsync();

        result.Id = calculation.Id;
        return result;
    }

    public async Task<IEnumerable<PowerCalculationResponse>> GetUserCalculationsAsync(int userId)
    {
        var calculations = await _calculationRepository.GetByUserIdAndTypeAsync(
            userId,
            CalculationType.Power
        );

        var results = new List<PowerCalculationResponse>();

        foreach (var calculation in calculations)
        {
            var response = JsonSerializer.Deserialize<PowerCalculationResponse>(
                calculation.ResultData,
                _jsonOptions
            );

            if (response != null)
            {
                response.Id = calculation.Id;
                response.Name = calculation.Name;
                results.Add(response);
            }
        }

        return results;
    }

    public async Task<PowerCalculationResponse?> GetByIdAsync(int id)
    {
        var calculation = await _calculationRepository.GetByIdAsync(id);

        if (calculation == null || calculation.CalculationType != CalculationType.Power)
        {
            return null;
        }

        var response = JsonSerializer.Deserialize<PowerCalculationResponse>(
            calculation.ResultData,
            _jsonOptions
        );

        if (response != null)
        {
            response.Id = calculation.Id;
            response.Name = calculation.Name;
        }

        return response;
    }
}