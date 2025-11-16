using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ucelo.Application.DTOs.Calculations;
using Ucelo.Application.Services.Interfaces;

namespace Ucelo.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CalculationsController : ControllerBase
{
    private readonly IPowerCalculationService _powerCalculationService;
    private readonly IComparisonCalculationService _comparisonCalculationService;
    private readonly ILogger<CalculationsController> _logger;

    public CalculationsController(
        IPowerCalculationService powerCalculationService,
        IComparisonCalculationService comparisonCalculationService,
        ILogger<CalculationsController> logger)
    {
        _powerCalculationService = powerCalculationService;
        _comparisonCalculationService = comparisonCalculationService;
        _logger = logger;
    }

    [HttpPost("power/calculate")]
    [ProducesResponseType(typeof(PowerCalculationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CalculatePower([FromBody] PowerCalculationRequest request)
    {
        try
        {
            var result = await _powerCalculationService.CalculateAsync(request);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Dados de entrada inválidos para cálculo de potência");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao calcular potência");
            return BadRequest(new { message = "Erro ao realizar cálculo de potência" });
        }
    }

    [HttpPost("power/save")]
    [ProducesResponseType(typeof(PowerCalculationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SavePowerCalculation([FromBody] PowerCalculationRequest request)
    {
        try
        {
            // Obter o ID do usuário autenticado
            var userId = GetUserId();

            var result = await _powerCalculationService.SaveCalculationAsync(userId, request);

            return CreatedAtAction(
                nameof(GetCalculationById),
                new { id = result.Id },
                result
            );
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Dados de entrada inválidos para cálculo de potência");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao salvar cálculo de potência");
            return BadRequest(new { message = "Erro ao salvar cálculo de potência" });
        }
    }

    [HttpGet("power/user")]
    [ProducesResponseType(typeof(IEnumerable<PowerCalculationResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserPowerCalculations()
    {
        try
        {
            var userId = GetUserId();
            var calculations = await _powerCalculationService.GetUserCalculationsAsync(userId);
            return Ok(calculations);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao recuperar cálculos de potência do usuário");
            return BadRequest(new { message = "Erro ao recuperar cálculos" });
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PowerCalculationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCalculationById(int id)
    {
        try
        {
            var calculation = await _powerCalculationService.GetByIdAsync(id);

            if (calculation == null)
                return NotFound(new { message = "Cálculo não encontrado" });

            return Ok(calculation);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao recuperar cálculo por ID: {Id}", id);
            return BadRequest(new { message = "Erro ao recuperar cálculo" });
        }
    }

    [HttpPost("comparison/calculate")]
    [ProducesResponseType(typeof(ComparisonCalculationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CalculateComparison([FromBody] ComparisonCalculationRequest request)
    {
        try
        {
            var result = await _comparisonCalculationService.CalculateAsync(request);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Dados de entrada inválidos para cálculo comparativo de canecas");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao calcular comparativo de canecas");
            return BadRequest(new { message = "Erro ao realizar cálculo comparativo de canecas" });
        }
    }

    [HttpPost("comparison/save")]
    [ProducesResponseType(typeof(ComparisonCalculationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SaveComparisonCalculation([FromBody] ComparisonCalculationRequest request)
    {
        try
        {
            var userId = GetUserId();
            var result = await _comparisonCalculationService.SaveCalculationAsync(userId, request);

            return CreatedAtAction(
                nameof(GetComparisonById),
                new { id = result.Id },
                result
            );
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Dados de entrada inválidos para cálculo comparativo de canecas");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao salvar cálculo comparativo de canecas");
            return BadRequest(new { message = "Erro ao salvar cálculo comparativo de canecas" });
        }
    }

    [HttpGet("comparison/user")]
    [ProducesResponseType(typeof(IEnumerable<ComparisonCalculationResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserComparisonCalculations()
    {
        try
        {
            var userId = GetUserId();
            var calculations = await _comparisonCalculationService.GetUserCalculationsAsync(userId);
            return Ok(calculations);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao recuperar cálculos comparativos do usuário");
            return BadRequest(new { message = "Erro ao recuperar cálculos comparativos" });
        }
    }

    [HttpGet("comparison/{id}")]
    [ProducesResponseType(typeof(ComparisonCalculationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetComparisonById(int id)
    {
        try
        {
            var calculation = await _comparisonCalculationService.GetByIdAsync(id);

            if (calculation == null)
                return NotFound(new { message = "Cálculo comparativo não encontrado" });

            return Ok(calculation);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao recuperar cálculo comparativo por ID: {Id}", id);
            return BadRequest(new { message = "Erro ao recuperar cálculo comparativo" });
        }
    }

    // Helper para obter o ID do usuário autenticado
    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
        {
            throw new InvalidOperationException("ID do usuário não encontrado ou inválido");
        }

        return userId;
    }
}