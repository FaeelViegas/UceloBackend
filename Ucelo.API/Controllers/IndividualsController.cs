using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ucelo.Application.DTOs.Individuals;
using Ucelo.Application.Services.Interfaces;

namespace Ucelo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IndividualsController : ControllerBase
{
    private readonly IIndividualService _individualService;
    private readonly ILogger<IndividualsController> _logger;

    public IndividualsController(
        IIndividualService individualService,
        ILogger<IndividualsController> logger)
    {
        _individualService = individualService;
        _logger = logger;
    }

    /// <summary>
    /// Criar um novo perfil fisico
    /// </summary>
    [HttpPost("user/{userId}")]
    [ProducesResponseType(typeof(IndividualResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create(int userId, [FromBody] CreateIndividualRequest request)
    {
        try
        {
            var individual = await _individualService.CreateAsync(userId, request);
            return CreatedAtAction(nameof(GetById), new { id = individual.Id }, individual);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Usuario não encontrado: {UserId}", userId);
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Falha ao criar perfil fisico: {UserId}", userId);
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Dado invalido para criar perfil");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Buscar por Id
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(IndividualResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var individual = await _individualService.GetByIdAsync(id);
            return Ok(individual);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Não encontrado: {Id}", id);
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Atualizar perfil fisico
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(IndividualResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] CreateIndividualRequest request)
    {
        try
        {
            var individual = await _individualService.UpdateAsync(id, request);
            return Ok(individual);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Perfil não encontrado: {Id}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Dado invalido");
            return BadRequest(new { message = ex.Message });
        }
    }
}