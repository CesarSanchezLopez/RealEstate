using RealEstate.Application.DTOs;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly IOwnerRepository _ownerRepository;

        public OwnerService(IOwnerRepository ownerRepository)
        {
            _ownerRepository = ownerRepository;
        }

        public async Task<OwnerDto> CreateAsync(OwnerDto dto)
        {
            var owner = new Owner
            {
                Name = dto.Name,
                Address = dto.Address,
                Photo = dto.Photo,
                Birthday = dto.Birthday
            };

            await _ownerRepository.AddAsync(owner);

            // Actualizamos el DTO con el Id generado
            dto.IdOwner = owner.IdOwner;
            return dto;
        }

        public async Task<IEnumerable<OwnerDto>> GetAllAsync()
        {
            var owners = await _ownerRepository.GetAllAsync();
            return owners.Select(o => new OwnerDto
            {
                IdOwner = o.IdOwner,
                Name = o.Name,
                Address = o.Address,
                Photo = o.Photo,
                Birthday = o.Birthday
            });
        }

        public async Task<OwnerDto?> GetByIdAsync(int id)
        {
            var owner = await _ownerRepository.GetByIdAsync(id);
            if (owner == null) return null;

            return new OwnerDto
            {
                IdOwner = owner.IdOwner,
                Name = owner.Name,
                Address = owner.Address,
                Photo = owner.Photo,
                Birthday = owner.Birthday
            };
        }
    }
}
