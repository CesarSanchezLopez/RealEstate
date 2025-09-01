using RealEstate.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.Interfaces
{
    public interface IOwnerService
    {
        Task<OwnerDto> CreateAsync(OwnerDto dto);
        Task<IEnumerable<OwnerDto>> GetAllAsync();
        Task<OwnerDto?> GetByIdAsync(int id);
    }
}
