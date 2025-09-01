using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.DTOs;
using RealEstate.Application.Interfaces;
using RealEstate.Infrastructure.Repositories;

namespace RealEstate.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService _propertyService;

        public PropertyController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        // ========================
        // Create Property
        // ========================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PropertyDto dto)
        {
            var id = await _propertyService.CreatePropertyAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, dto);
        }

        // ========================
        // Add Image to Property
        // ========================
        [HttpPost("{propertyId}/images")]
        public async Task<IActionResult> AddImage(int propertyId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);

            var dto = new PropertyImageDto
            {
                IdProperty = propertyId,
                File = ms.ToArray()
            };

            await _propertyService.AddImageAsync(dto);
            return Ok();
        }

        // ========================
        // Change Price
        // ========================
        [HttpPatch("{propertyId}/price")]
        public async Task<IActionResult> ChangePrice(int propertyId, [FromBody] decimal newPrice, string changedBy)
        {
            await _propertyService.ChangePriceAsync(propertyId, newPrice,changedBy);
            return Ok("Price updated and trace recorded.");
        }

        // ========================
        // Update Property
        // ========================
        [HttpPut("{propertyId}")]
        public async Task<IActionResult> Update(int propertyId, [FromBody] PropertyDto dto)
        {
            dto.IdProperty = propertyId;
            await _propertyService.UpdateAsync(dto);
            return NoContent();
        }

        // ========================
        // List / Filter Properties
        // ========================
        [HttpGet]
        public async Task<IActionResult> GetFiltered(
            [FromQuery] string? name,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice)
        {
            var properties = await _propertyService.GetFilteredAsync(name, minPrice, maxPrice);
            return Ok(properties);
        }

        // ========================
        // Get Property by Id
        // ========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var property = await _propertyService.GetByIdAsync(id);
          
            return Ok(property);
        }

       
    }
}
