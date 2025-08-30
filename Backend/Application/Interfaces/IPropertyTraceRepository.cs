using RealEstate.Domain.Entities;

namespace RealEstate.Application.Interfaces
{
    public interface IPropertyTraceRepository
    {
        Task AddAsync(PropertyTrace trace);
        Task<IEnumerable<PropertyTrace>> GetByPropertyIdAsync(int propertyId);
    }
}
