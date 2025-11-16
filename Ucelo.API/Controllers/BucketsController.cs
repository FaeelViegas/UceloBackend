using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ucelo.Application.DTOs.Calculations;
using Ucelo.Domain.Interfaces;

namespace Ucelo.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BucketsController : ControllerBase
    {
        private readonly IBucketRepository _bucketRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly ILogger<BucketsController> _logger;

        public BucketsController(
            IBucketRepository bucketRepository,
            IMaterialRepository materialRepository,
            ILogger<BucketsController> logger)
        {
            _bucketRepository = bucketRepository;
            _materialRepository = materialRepository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BucketListItemDto>), 200)]
        public async Task<IActionResult> GetAllBuckets()
        {
            try
            {
                var buckets = await _bucketRepository.GetAllActiveAsync();
                var materials = await _materialRepository.GetAllActiveAsync();

                var materialsDict = materials.ToDictionary(m => m.Id, m => m.Nome);
                
                var result = buckets.Select(b => new BucketListItemDto
                {
                    Id = b.Id,
                    Code = b.Codigo,
                    Dimensions = b.Dimensions,
                    MaterialId = b.MaterialId,
                    MaterialName = materialsDict.TryGetValue(b.MaterialId, out var materialName) ? materialName : string.Empty
                });

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter lista de canecas");
                return BadRequest(new { message = "Erro ao obter lista de canecas" });
            }
        }

        [HttpGet("materials")]
        [ProducesResponseType(typeof(IEnumerable<MaterialDto>), 200)]
        public async Task<IActionResult> GetAllMaterials()
        {
            try
            {
                var materials = await _materialRepository.GetAllActiveAsync();
                
                var result = materials.Select(m => new MaterialDto
                {
                    Id = m.Id,
                    Name = m.Nome
                });

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter lista de materiais");
                return BadRequest(new { message = "Erro ao obter lista de materiais" });
            }
        }
    }
}