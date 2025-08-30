using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Repositories
{
    public class PropertyImageRepository : IPropertyImageRepository
    {
        private readonly RealEstateDbContext _context;

        public PropertyImageRepository(RealEstateDbContext context)
        {
            _context = context;
        }

        public async Task<PropertyImage?> GetByIdAsync(int id)
        {
            return await _context.PropertyImages
                .FirstOrDefaultAsync(i => i.IdPropertyImage == id);
        }

        public async Task AddAsync(PropertyImage image)
        {
            await _context.PropertyImages.AddAsync(image);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PropertyImage image)
        {
            _context.PropertyImages.Update(image);
            await _context.SaveChangesAsync();
        }
    }
}
