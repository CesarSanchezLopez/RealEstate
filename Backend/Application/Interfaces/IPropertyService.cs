using RealEstate.Application.DTOs;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.Interfaces
{
    public interface IPropertyService
    {
        Task<int> CreatePropertyAsync(PropertyDto dto);
        Task AddImageAsync(PropertyImageDto dto);
        Task ChangePriceAsync(int propertyId, decimal newPrice);
        Task UpdateAsync(PropertyDto dto);
        Task<IEnumerable<PropertyDto>> GetFilteredAsync(string? name, decimal? minPrice, decimal? maxPrice);
    }
}
