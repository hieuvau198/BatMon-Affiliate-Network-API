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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AdvertiserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<AdvertiserDto>> GetAllAdvertisersAsync()
        {
            var advertisers = await _unitOfWork.Advertisers.GetAllAsync();
            return _mapper.Map<IEnumerable<AdvertiserDto>>(advertisers);
        }

        public async Task<AdvertiserDto> GetAdvertiserByIdAsync(int id, bool includeRelated = false)
        {
            Advertiser advertiser;

            if (includeRelated)
            {
                advertiser = await _unitOfWork.Advertisers.GetByIdAsync(id,
                    a => a.AdvertiserUrls,
                    a => a.Campaigns,
                    a => a.AdvertiserBalances);
            }
            else
            {
                advertiser = await _unitOfWork.Advertisers.GetByIdAsync(id);
            }

            return _mapper.Map<AdvertiserDto>(advertiser);
        }

        public async Task<AdvertiserDto> GetAdvertiserByEmailAsync(string email)
        {
            var advertiser = await _unitOfWork.Advertisers.FindAsync(a => a.Email == email);
            return _mapper.Map<AdvertiserDto>(advertiser);
        }

        public async Task<AdvertiserDto> CreateAdvertiserAsync(AdvertiserCreateDto advertiserDto)
        {
            // Check if email already exists
            if (await _unitOfWork.Advertisers.ExistsAsync(a => a.Email == advertiserDto.Email))
            {
                throw new InvalidOperationException($"Email {advertiserDto.Email} is already registered");
            }

            var advertiser = _mapper.Map<Advertiser>(advertiserDto);
            advertiser.RegistrationDate = DateOnly.FromDateTime(DateTime.UtcNow);
            advertiser.IsActive = true;
            advertiser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(advertiserDto.Password);

            var createdAdvertiser = await _unitOfWork.Advertisers.CreateAsync(advertiser);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AdvertiserDto>(createdAdvertiser);
        }

        public async Task<AdvertiserDto> UpdateAdvertiserAsync(AdvertiserUpdateDto advertiserDto)
        {
            var existingAdvertiser = await _unitOfWork.Advertisers.GetByIdAsync(advertiserDto.AdvertiserId);
            if (existingAdvertiser == null)
            {
                throw new KeyNotFoundException($"Advertiser with ID {advertiserDto.AdvertiserId} not found");
            }

            if (existingAdvertiser.Email != advertiserDto.Email &&
                await _unitOfWork.Advertisers.ExistsAsync(a => a.Email == advertiserDto.Email && a.AdvertiserId != advertiserDto.AdvertiserId))
            {
                throw new InvalidOperationException($"Email {advertiserDto.Email} is already registered");
            }

            _mapper.Map(advertiserDto, existingAdvertiser);
            await _unitOfWork.Advertisers.UpdateAsync(existingAdvertiser);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AdvertiserDto>(existingAdvertiser);
        }

        public async Task DeleteAdvertiserAsync(int id)
        {
            var advertiser = await _unitOfWork.Advertisers.GetByIdAsync(id);
            if (advertiser == null)
            {
                throw new KeyNotFoundException($"Advertiser with ID {id} not found");
            }

            await _unitOfWork.Advertisers.DeleteAsync(advertiser);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ActivateAdvertiserAsync(int id)
        {
            var advertiser = await _unitOfWork.Advertisers.GetByIdAsync(id);
            if (advertiser == null)
            {
                throw new KeyNotFoundException($"Advertiser with ID {id} not found");
            }

            advertiser.IsActive = true;
            await _unitOfWork.Advertisers.UpdateAsync(advertiser);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeactivateAdvertiserAsync(int id)
        {
            var advertiser = await _unitOfWork.Advertisers.GetByIdAsync(id);
            if (advertiser == null)
            {
                throw new KeyNotFoundException($"Advertiser with ID {id} not found");
            }

            advertiser.IsActive = false;
            await _unitOfWork.Advertisers.UpdateAsync(advertiser);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AdvertiserExistsAsync(int id)
        {
            return await _unitOfWork.Advertisers.ExistsAsync(a => a.AdvertiserId == id);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _unitOfWork.Advertisers.ExistsAsync(a => a.Email == email);
        }

        public async Task<int> GetAdvertiserCountAsync()
        {
            return await _unitOfWork.Advertisers.CountAsync(a => true);
        }
    }
}