using RealEstate.Domain.Entities;

namespace RealEstate.Application.Interfaces
{
    public interface IOwnerRepository
    {
        Task<Owner?> GetByIdAsync(int id);
        Task<IEnumerable<Owner>> GetAllAsync();
        Task AddAsync(Owner owner);
        Task UpdateAsync(Owner owner);
    }
}
