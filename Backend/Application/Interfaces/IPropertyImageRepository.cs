using RealEstate.Domain.Entities;

namespace RealEstate.Application.Interfaces
{
    public interface IPropertyImageRepository
    {
        Task<PropertyImage?> GetByIdAsync(int id);
        Task AddAsync(PropertyImage image);
        Task UpdateAsync(PropertyImage image);
    }
}
