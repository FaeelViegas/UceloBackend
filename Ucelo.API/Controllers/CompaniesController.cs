using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ucelo.Application.DTOs.Companies;
using Ucelo.Application.Services.Interfaces;

namespace Ucelo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyService _companyService;
    private readonly ILogger<CompaniesController> _logger;

    public CompaniesController(
        ICompanyService companyService,
        ILogger<CompaniesController> logger)
    {
        _companyService = companyService;
        _logger = logger;
    }

    /// <summary>
    /// Cria um novo perfil juridico
    /// </summary>
    [HttpPost("user/{userId}")]
    [ProducesResponseType(typeof(CompanyResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create(int userId, [FromBody] CreateCompanyRequest request)
    {
        try
        {
            var company = await _companyService.CreateAsync(userId, request);
            return CreatedAtAction(nameof(GetById), new { id = company.Id }, company);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Usuario não encontrado: {UserId}", userId);
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Falha ao criar perfil juridico: {UserId}", userId);
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Dado invalido para perfil juridico");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Busca por Id
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CompanyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var company = await _companyService.GetByIdAsync(id);
            return Ok(company);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Company not found: {Id}", id);
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Buscar por CNPJ
    /// </summary>
    [HttpGet("tax-id/{taxId}")]
    [ProducesResponseType(typeof(CompanyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByTaxId(string taxId)
    {
        var company = await _companyService.GetByTaxIdAsync(taxId);
        
        if (company == null)
            return NotFound(new { message = "Empresa não encontrada" });

        return Ok(company);
    }

    /// <summary>
    /// Busca todos os perfis juridicos
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CompanyResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var companies = await _companyService.GetAllAsync();
        return Ok(companies);
    }

    /// <summary>
    /// Busca todos os perfis juridicos ativos
    /// </summary>
    [HttpGet("active")]
    [ProducesResponseType(typeof(IEnumerable<CompanyResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActiveCompanies()
    {
        var companies = await _companyService.GetActiveCompaniesAsync();
        return Ok(companies);
    }

    /// <summary>
    /// Atualizar perfil juridico
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(CompanyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] CreateCompanyRequest request)
    {
        try
        {
            var company = await _companyService.UpdateAsync(id, request);
            return Ok(company);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Perfil não encontrado: {Id}", id);
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Dado Invalido");
            return BadRequest(new { message = ex.Message });
        }
    }
    
}