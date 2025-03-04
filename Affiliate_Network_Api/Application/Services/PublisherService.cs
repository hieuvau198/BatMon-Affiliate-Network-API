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
        private readonly IGenericRepository<Publisher> _publisherRepository;
        private readonly IMapper _mapper;

        public PublisherService(IGenericRepository<Publisher> publisherRepository, IMapper mapper)
        {
            _publisherRepository = publisherRepository ?? throw new ArgumentNullException(nameof(publisherRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<PublisherDto>> GetAllPublishersAsync()
        {
            var publishers = await _publisherRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PublisherDto>>(publishers);
        }

        public async Task<PublisherDto> GetPublisherByIdAsync(int id, bool includeRelated = false)
        {
            Publisher publisher;

            if (includeRelated)
            {
                publisher = await _publisherRepository.GetByIdAsync(id,
                    p => p.PublisherBalances,
                    p => p.PublisherProfiles,
                    p => p.TrafficSources,
                    p => p.CampaignPublisherCommissions);
            }
            else
            {
                publisher = await _publisherRepository.GetByIdAsync(id);
            }

            if (publisher == null)
            {
                throw new KeyNotFoundException($"Publisher with ID {id} not found");
            }

            return _mapper.Map<PublisherDto>(publisher);
        }

        public async Task<PublisherDto> GetPublisherByEmailAsync(string email)
        {
            var publisher = await _publisherRepository.FindAsync(p => p.Email == email);
            if (publisher == null)
            {
                throw new KeyNotFoundException($"Publisher with email {email} not found");
            }

            return _mapper.Map<PublisherDto>(publisher);
        }

        public async Task<PublisherDto> GetPublisherByUsernameAsync(string username)
        {
            var publisher = await _publisherRepository.FindAsync(p => p.Username == username);
            if (publisher == null)
            {
                throw new KeyNotFoundException($"Publisher with username {username} not found");
            }

            return _mapper.Map<PublisherDto>(publisher);
        }

        public async Task<PublisherDto> CreatePublisherAsync(PublisherCreateDto publisherDto)
        {
            // Check if email already exists
            if (await _publisherRepository.ExistsAsync(p => p.Email == publisherDto.Email))
            {
                throw new InvalidOperationException($"Email {publisherDto.Email} is already registered");
            }

            // Check if username already exists
            if (await _publisherRepository.ExistsAsync(p => p.Username == publisherDto.Username))
            {
                throw new InvalidOperationException($"Username {publisherDto.Username} is already taken");
            }

            // Map the create DTO to entity
            var publisher = _mapper.Map<Publisher>(publisherDto);

            // Set default values
            publisher.RegistrationDate = DateOnly.FromDateTime(DateTime.UtcNow);
            publisher.IsActive = true;

            // Generate unique referral code if not provided
            if (string.IsNullOrEmpty(publisher.ReferralCode))
            {
                publisher.ReferralCode = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            }

            // Hash the password (this should use a proper password hashing service)
            publisher.PasswordHash = BCrypt.Net.BCrypt.HashPassword(publisherDto.Password);

            // Save to database
            var createdPublisher = await _publisherRepository.CreateAsync(publisher);

            return _mapper.Map<PublisherDto>(createdPublisher);
        }

        public async Task<PublisherDto> UpdatePublisherAsync(PublisherUpdateDto publisherDto)
        {
            // First get the existing publisher
            var existingPublisher = await _publisherRepository.GetByIdAsync(publisherDto.PublisherId);
            if (existingPublisher == null)
            {
                throw new KeyNotFoundException($"Publisher with ID {publisherDto.PublisherId} not found");
            }

            // Check if email is being changed and if it's already in use
            if (!string.IsNullOrEmpty(publisherDto.Email) &&
                existingPublisher.Email != publisherDto.Email &&
                await _publisherRepository.ExistsAsync(p => p.Email == publisherDto.Email && p.PublisherId != publisherDto.PublisherId))
            {
                throw new InvalidOperationException($"Email {publisherDto.Email} is already registered");
            }

            // Check if username is being changed and if it's already in use
            if (!string.IsNullOrEmpty(publisherDto.Username) &&
                existingPublisher.Username != publisherDto.Username &&
                await _publisherRepository.ExistsAsync(p => p.Username == publisherDto.Username && p.PublisherId != publisherDto.PublisherId))
            {
                throw new InvalidOperationException($"Username {publisherDto.Username} is already taken");
            }

            // Map updated properties, preserving the ones we don't want to change
            _mapper.Map(publisherDto, existingPublisher);

            // Update in database
            await _publisherRepository.UpdateAsync(existingPublisher);

            return _mapper.Map<PublisherDto>(existingPublisher);
        }

        public async Task DeletePublisherAsync(int id)
        {
            var publisher = await _publisherRepository.GetByIdAsync(id);
            if (publisher == null)
            {
                throw new KeyNotFoundException($"Publisher with ID {id} not found");
            }

            await _publisherRepository.DeleteAsync(publisher);
        }

        public async Task<bool> ActivatePublisherAsync(int id)
        {
            var publisher = await _publisherRepository.GetByIdAsync(id);
            if (publisher == null)
            {
                throw new KeyNotFoundException($"Publisher with ID {id} not found");
            }

            publisher.IsActive = true;
            await _publisherRepository.UpdateAsync(publisher);
            return true;
        }

        public async Task<bool> DeactivatePublisherAsync(int id)
        {
            var publisher = await _publisherRepository.GetByIdAsync(id);
            if (publisher == null)
            {
                throw new KeyNotFoundException($"Publisher with ID {id} not found");
            }

            publisher.IsActive = false;
            await _publisherRepository.UpdateAsync(publisher);
            return true;
        }

        public async Task<bool> PublisherExistsAsync(int id)
        {
            return await _publisherRepository.ExistsAsync(p => p.PublisherId == id);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _publisherRepository.ExistsAsync(p => p.Email == email);
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _publisherRepository.ExistsAsync(p => p.Username == username);
        }

        public async Task<int> GetPublisherCountAsync()
        {
            return await _publisherRepository.CountAsync(p => true);
        }

        public async Task<IEnumerable<PublisherDto>> GetPublishersByReferrerCodeAsync(string referralCode)
        {
            var publishers = await _publisherRepository.GetAllAsync(p => p.ReferredByCode == referralCode);
            return _mapper.Map<IEnumerable<PublisherDto>>(publishers);
        }

        public async Task<int> GetReferralCountAsync(string referralCode)
        {
            return await _publisherRepository.CountAsync(p => p.ReferredByCode == referralCode);
        }
    }
}