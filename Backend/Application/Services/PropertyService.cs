using RealEstate.Application.DTOs;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPropertyImageRepository _propertyImageRepository;

        public PropertyService(
            IPropertyRepository propertyRepository,
            IPropertyImageRepository propertyImageRepository)
        {
            _propertyRepository = propertyRepository;
            _propertyImageRepository = propertyImageRepository;
        }

        public async Task<int> CreatePropertyAsync(PropertyDto dto)
        {
            var property = new Property
            {
                Name = dto.Name,
                Address = dto.Address,
                Price = dto.Price,
                CodeInternal = dto.CodeInternal,
                Year = dto.Year,
                IdOwner = dto.IdOwner
            };

            await _propertyRepository.AddAsync(property);
            return property.IdProperty;
        }

        public async Task AddImageAsync(PropertyImageDto dto)
        {
            var image = new PropertyImage
            {
                IdProperty = dto.IdProperty,
                File = dto.File,
                Enabled = dto.Enabled
            };

            await _propertyImageRepository.AddAsync(image);
        }

        public async Task ChangePriceAsync(int propertyId, decimal newPrice)
        {
            var property = await _propertyRepository.GetByIdAsync(propertyId);

            if (property == null)
                throw new KeyNotFoundException($"Property {propertyId} not found");

            property.Price = newPrice;
            await _propertyRepository.UpdateAsync(property);
        }

        public async Task UpdateAsync(PropertyDto dto)
        {
            var property = await _propertyRepository.GetByIdAsync(dto.IdProperty);

            if (property == null)
                throw new KeyNotFoundException($"Property {dto.IdProperty} not found");

            property.Name = dto.Name;
            property.Address = dto.Address;
            property.Price = dto.Price;
            property.CodeInternal = dto.CodeInternal;
            property.Year = dto.Year;
            property.IdOwner = dto.IdOwner;

            await _propertyRepository.UpdateAsync(property);
        }

        public async Task<IEnumerable<PropertyDto>> GetFilteredAsync(string? name, decimal? minPrice, decimal? maxPrice)
        {
            var properties = await _propertyRepository.GetFilteredAsync(name, minPrice, maxPrice);

            return properties.Select(p => new PropertyDto
            {
                IdProperty = p.IdProperty,
                Name = p.Name,
                Address = p.Address,
                Price = p.Price,
                CodeInternal = p.CodeInternal,
                Year = p.Year,
                IdOwner = p.IdOwner
            });
        }
    }
}
