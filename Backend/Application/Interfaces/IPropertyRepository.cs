using RealEstate.Domain.Entities;

namespace RealEstate.Application.Interfaces
{
    public interface IPropertyRepository
    {
        Task<Property?> GetByIdAsync(int id);
        Task<IEnumerable<Property>> GetFilteredAsync(string? name, decimal? minPrice, decimal? maxPrice);
        Task AddAsync(Property property);
        Task UpdateAsync(Property property);
        Task AddTraceAsync(PropertyTrace trace);
    }
}
