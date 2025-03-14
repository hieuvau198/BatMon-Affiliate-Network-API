using Application.Contracts.Publisher;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PublisherService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<PublisherDto>> GetAllPublishersAsync()
        {
            var publishers = await _unitOfWork.Publishers.GetAllAsync();
            return _mapper.Map<IEnumerable<PublisherDto>>(publishers);
        }

        public async Task<PublisherDto> GetPublisherByIdAsync(int id, bool includeRelated = false)
        {
            Publisher publisher;

            if (includeRelated)
            {
                publisher = await _unitOfWork.Publishers.GetByIdAsync(id,
                    p => p.PublisherBalances,
                    p => p.PublisherProfiles,
                    p => p.TrafficSources,
                    p => p.CampaignPublisherCommissions);
            }
            else
            {
                publisher = await _unitOfWork.Publishers.GetByIdAsync(id);
            }

            if (publisher == null)
            {
                throw new KeyNotFoundException($"Publisher with ID {id} not found");
            }

            return _mapper.Map<PublisherDto>(publisher);
        }

        public async Task<PublisherDto> GetPublisherByEmailAsync(string email)
        {
            var publisher = await _unitOfWork.Publishers.FindAsync(p => p.Email == email);
            if (publisher == null)
            {
                throw new KeyNotFoundException($"Publisher with email {email} not found");
            }

            return _mapper.Map<PublisherDto>(publisher);
        }

        public async Task<PublisherDto> GetPublisherByUsernameAsync(string username)
        {
            var publisher = await _unitOfWork.Publishers.FindAsync(p => p.Username == username);
            if (publisher == null)
            {
                throw new KeyNotFoundException($"Publisher with username {username} not found");
            }

            return _mapper.Map<PublisherDto>(publisher);
        }

        public async Task<PublisherDto> CreatePublisherAsync(PublisherCreateDto publisherDto)
        {
            if (await _unitOfWork.Publishers.ExistsAsync(p => p.Email == publisherDto.Email))
            {
                throw new InvalidOperationException($"Email {publisherDto.Email} is already registered");
            }

            if (await _unitOfWork.Publishers.ExistsAsync(p => p.Username == publisherDto.Username))
            {
                throw new InvalidOperationException($"Username {publisherDto.Username} is already taken");
            }

            var publisher = _mapper.Map<Publisher>(publisherDto);

            publisher.RegistrationDate = DateOnly.FromDateTime(DateTime.UtcNow);
            publisher.IsActive = true;

            if (string.IsNullOrEmpty(publisher.ReferralCode))
            {
                publisher.ReferralCode = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            }

            publisher.PasswordHash = BCrypt.Net.BCrypt.HashPassword(publisherDto.Password);

            var createdPublisher = await _unitOfWork.Publishers.CreateAsync(publisher);

            return _mapper.Map<PublisherDto>(createdPublisher);
        }

        public async Task<PublisherDto> UpdatePublisherAsync(PublisherUpdateDto publisherDto)
        {
            var existingPublisher = await _unitOfWork.Publishers.GetByIdAsync(publisherDto.PublisherId);
            if (existingPublisher == null)
            {
                throw new KeyNotFoundException($"Publisher with ID {publisherDto.PublisherId} not found");
            }

            if (!string.IsNullOrEmpty(publisherDto.Email) &&
                existingPublisher.Email != publisherDto.Email &&
                await _unitOfWork.Publishers.ExistsAsync(p => p.Email == publisherDto.Email && p.PublisherId != publisherDto.PublisherId))
            {
                throw new InvalidOperationException($"Email {publisherDto.Email} is already registered");
            }

            if (!string.IsNullOrEmpty(publisherDto.Username) &&
                existingPublisher.Username != publisherDto.Username &&
                await _unitOfWork.Publishers.ExistsAsync(p => p.Username == publisherDto.Username && p.PublisherId != publisherDto.PublisherId))
            {
                throw new InvalidOperationException($"Username {publisherDto.Username} is already taken");
            }

            _mapper.Map(publisherDto, existingPublisher);

            await _unitOfWork.Publishers.UpdateAsync(existingPublisher);

            return _mapper.Map<PublisherDto>(existingPublisher);
        }

        public async Task DeletePublisherAsync(int id)
        {
            var publisher = await _unitOfWork.Publishers.GetByIdAsync(id);
            if (publisher == null)
            {
                throw new KeyNotFoundException($"Publisher with ID {id} not found");
            }

            await _unitOfWork.Publishers.DeleteAsync(publisher);
        }

        public async Task<bool> ActivatePublisherAsync(int id)
        {
            var publisher = await _unitOfWork.Publishers.GetByIdAsync(id);
            if (publisher == null)
            {
                throw new KeyNotFoundException($"Publisher with ID {id} not found");
            }

            publisher.IsActive = true;
            await _unitOfWork.Publishers.UpdateAsync(publisher);
            return true;
        }

        public async Task<bool> DeactivatePublisherAsync(int id)
        {
            var publisher = await _unitOfWork.Publishers.GetByIdAsync(id);
            if (publisher == null)
            {
                throw new KeyNotFoundException($"Publisher with ID {id} not found");
            }

            publisher.IsActive = false;
            await _unitOfWork.Publishers.UpdateAsync(publisher);
            return true;
        }

        public async Task<bool> PublisherExistsAsync(int id)
        {
            return await _unitOfWork.Publishers.ExistsAsync(p => p.PublisherId == id);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _unitOfWork.Publishers.ExistsAsync(p => p.Email == email);
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _unitOfWork.Publishers.ExistsAsync(p => p.Username == username);
        }

        public async Task<int> GetPublisherCountAsync()
        {
            return await _unitOfWork.Publishers.CountAsync(p => true);
        }

        public async Task<IEnumerable<PublisherDto>> GetPublishersByReferrerCodeAsync(string referralCode)
        {
            var publishers = await _unitOfWork.Publishers.GetAllAsync(p => p.ReferredByCode == referralCode);
            return _mapper.Map<IEnumerable<PublisherDto>>(publishers);
        }

        public async Task<int> GetReferralCountAsync(string referralCode)
        {
            return await _unitOfWork.Publishers.CountAsync(p => p.ReferredByCode == referralCode);
        }
    }
}