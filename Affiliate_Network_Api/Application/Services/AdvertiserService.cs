using Application.Contracts.Advertiser;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AdvertiserService : IAdvertiserService
    {
        private readonly IGenericRepository<Advertiser> _advertiserRepository;
        private readonly IMapper _mapper;

        public AdvertiserService(IGenericRepository<Advertiser> advertiserRepository, IMapper mapper)
        {
            _advertiserRepository = advertiserRepository ?? throw new ArgumentNullException(nameof(advertiserRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<AdvertiserDto>> GetAllAdvertisersAsync()
        {
            var advertisers = await _advertiserRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<AdvertiserDto>>(advertisers);
        }

        public async Task<AdvertiserDto> GetAdvertiserByIdAsync(int id, bool includeRelated = false)
        {
            Advertiser advertiser;

            if (includeRelated)
            {
                advertiser = await _advertiserRepository.GetByIdAsync(id,
                    a => a.AdvertiserUrls,
                    a => a.Campaigns,
                    a => a.AdvertiserBalances);
            }
            else
            {
                advertiser = await _advertiserRepository.GetByIdAsync(id);
            }

            return _mapper.Map<AdvertiserDto>(advertiser);
        }

        public async Task<AdvertiserDto> GetAdvertiserByEmailAsync(string email)
        {
            var advertiser = await _advertiserRepository.FindAsync(a => a.Email == email);
            return _mapper.Map<AdvertiserDto>(advertiser);
        }

        public async Task<AdvertiserDto> CreateAdvertiserAsync(AdvertiserCreateDto advertiserDto)
        {
            // Check if email already exists
            if (await _advertiserRepository.ExistsAsync(a => a.Email == advertiserDto.Email))
            {
                throw new InvalidOperationException($"Email {advertiserDto.Email} is already registered");
            }

            // Map the create DTO to entity
            var advertiser = _mapper.Map<Advertiser>(advertiserDto);

            // Set default values
            advertiser.RegistrationDate = DateOnly.FromDateTime(DateTime.UtcNow);
            advertiser.IsActive = true;

            // Hash the password (this should use a proper password hashing service)
            advertiser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(advertiserDto.Password);

            // Save to database
            var createdAdvertiser = await _advertiserRepository.CreateAsync(advertiser);

            return _mapper.Map<AdvertiserDto>(createdAdvertiser);
        }

        public async Task<AdvertiserDto> UpdateAdvertiserAsync(AdvertiserUpdateDto advertiserDto)
        {
            // First get the existing advertiser
            var existingAdvertiser = await _advertiserRepository.GetByIdAsync(advertiserDto.AdvertiserId);
            if (existingAdvertiser == null)
            {
                throw new KeyNotFoundException($"Advertiser with ID {advertiserDto.AdvertiserId} not found");
            }

            // Check if email is being changed and if it's already in use
            if (existingAdvertiser.Email != advertiserDto.Email &&
                await _advertiserRepository.ExistsAsync(a => a.Email == advertiserDto.Email && a.AdvertiserId != advertiserDto.AdvertiserId))
            {
                throw new InvalidOperationException($"Email {advertiserDto.Email} is already registered");
            }

            // Map updated properties, preserving the ones we don't want to change
            _mapper.Map(advertiserDto, existingAdvertiser);

            // Update in database
            await _advertiserRepository.UpdateAsync(existingAdvertiser);

            return _mapper.Map<AdvertiserDto>(existingAdvertiser);
        }

        public async Task DeleteAdvertiserAsync(int id)
        {
            var advertiser = await _advertiserRepository.GetByIdAsync(id);
            if (advertiser == null)
            {
                throw new KeyNotFoundException($"Advertiser with ID {id} not found");
            }

            await _advertiserRepository.DeleteAsync(advertiser);
        }

        public async Task<bool> ActivateAdvertiserAsync(int id)
        {
            var advertiser = await _advertiserRepository.GetByIdAsync(id);
            if (advertiser == null)
            {
                throw new KeyNotFoundException($"Advertiser with ID {id} not found");
            }

            advertiser.IsActive = true;
            await _advertiserRepository.UpdateAsync(advertiser);
            return true;
        }

        public async Task<bool> DeactivateAdvertiserAsync(int id)
        {
            var advertiser = await _advertiserRepository.GetByIdAsync(id);
            if (advertiser == null)
            {
                throw new KeyNotFoundException($"Advertiser with ID {id} not found");
            }

            advertiser.IsActive = false;
            await _advertiserRepository.UpdateAsync(advertiser);
            return true;
        }

        public async Task<bool> AdvertiserExistsAsync(int id)
        {
            return await _advertiserRepository.ExistsAsync(a => a.AdvertiserId == id);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _advertiserRepository.ExistsAsync(a => a.Email == email);
        }

        public async Task<int> GetAdvertiserCountAsync()
        {
            return await _advertiserRepository.CountAsync(a => true);
        }
    }
}