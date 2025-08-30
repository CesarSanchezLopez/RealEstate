using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.DTOs;
using RealEstate.Application.Interfaces;

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
        public async Task<IActionResult> AddImage(int propertyId, [FromBody] PropertyImageDto dto)
        {
            dto.IdProperty = propertyId;
            await _propertyService.AddImageAsync(dto);
            return Ok();
        }

        // ========================
        // Change Price
        // ========================
        [HttpPatch("{propertyId}/price")]
        public async Task<IActionResult> ChangePrice(int propertyId, [FromBody] decimal newPrice)
        {
            await _propertyService.ChangePriceAsync(propertyId, newPrice);
            return NoContent();
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
            var property = await _propertyService.GetFilteredAsync(null, null, null);
            var single = property.FirstOrDefault(p => p.IdProperty == id);
            if (single == null) return NotFound();
            return Ok(single);
        }
    }
}
