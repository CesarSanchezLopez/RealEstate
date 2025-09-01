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
                File = dto.File, // <-- conversión base64 -> byte[]
                Enabled = dto.Enabled
            };

            await _propertyImageRepository.AddAsync(image);
        }

        public async Task ChangePriceAsync(int propertyId, decimal newPrice, string changedBy)
        {
            var property = await _propertyRepository.GetByIdAsync(propertyId);

            if (property == null)
                throw new Exception("Property not found.");

            // Guardar el cambio en PropertyTrace
            var trace = new PropertyTrace
            {
                IdProperty = propertyId,
                DateSale = DateTime.UtcNow,
                Name = changedBy, // quién cambió el precio
                Value = newPrice,
                Tax = newPrice * 0.1m // ejemplo: 10% impuesto
            };

            await _propertyRepository.AddTraceAsync(trace);

            // Cambiar el precio en Property
            property.Price = newPrice;
            await _propertyRepository.UpdateAsync(property);
        }

        public async Task UpdateAsync(PropertyDto dto)
        {
            var property = await _propertyRepository.GetByIdAsync(dto.IdProperty);

            if (property == null)
                throw new KeyNotFoundException($"Property {dto.IdProperty} not found");

            // Opcional: Validaciones de negocio antes de actualizar
            if (dto.Price <= 0)
                throw new ArgumentException("Price must be greater than zero.");

            // Actualizar campos
            property.Name = dto.Name;
            property.Address = dto.Address;
            property.Price = dto.Price;
            property.CodeInternal = dto.CodeInternal;
            property.Year = dto.Year;
            property.IdOwner = dto.IdOwner;

            await _propertyRepository.UpdateAsync(property);


            var trace = new PropertyTrace
            {
                IdProperty = property.IdProperty,
                DateSale = DateTime.UtcNow,
                Name = "System Update",
                Value = dto.Price,
                Tax = dto.Price * 0.1m // ejemplo: 10%
            };

            await _propertyRepository.AddTraceAsync(trace);

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
                IdOwner = p.IdOwner,
                Image = p.Images != null && p.Images.Any()
                ? Convert.ToBase64String(p.Images.OrderByDescending(i => i.IdPropertyImage).First().File)
                : null
                //Image = p.Images != null && p.Images.Any()
                //    ? Convert.ToBase64String(p.Images.First().File)
                //    : null
            });
        }
        public async Task<PropertyDto?> GetByIdAsync(int id)
        {
            var property = await _propertyRepository.GetByIdAsync(id);
            if (property == null) return null;

            var dto = new PropertyDto
            {
                IdProperty = property.IdProperty,
                Name = property.Name,
                Address = property.Address,
                Price = property.Price,
                CodeInternal = property.CodeInternal,
                Year = property.Year,
                IdOwner = property.IdOwner,
                Traces = property.Traces.Select(t => new PropertyTraceDto
                {
                    IdPropertyTrace = t.IdPropertyTrace,
                    DateSale = t.DateSale,
                    Name = t.Name,
                    Value = t.Value,
                    Tax = t.Tax
                }).ToList()
            };

            return dto;
        }
    }
}
