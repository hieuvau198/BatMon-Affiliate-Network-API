using Application.Contracts.TrafficSource;
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
    public class TrafficSourceService : ITrafficSourceService
    {
        private readonly IGenericRepository<TrafficSource> _trafficSourceRepository;
        private readonly IGenericRepository<Publisher> _publisherRepository;
        private readonly IMapper _mapper;

        public TrafficSourceService(
            IGenericRepository<TrafficSource> trafficSourceRepository,
            IGenericRepository<Publisher> publisherRepository,
            IMapper mapper)
        {
            _trafficSourceRepository = trafficSourceRepository ?? throw new ArgumentNullException(nameof(trafficSourceRepository));
            _publisherRepository = publisherRepository ?? throw new ArgumentNullException(nameof(publisherRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<TrafficSourceDto>> GetAllTrafficSourcesAsync()
        {
            var trafficSources = await _trafficSourceRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TrafficSourceDto>>(trafficSources);
        }

        public async Task<TrafficSourceDto> GetTrafficSourceByIdAsync(int id, bool includeRelated = false)
        {
            TrafficSource trafficSource;

            if (includeRelated)
            {
                trafficSource = await _trafficSourceRepository.GetByIdAsync(id,
                    ts => ts.Publisher,
                    ts => ts.PromoteUrlvariants);
            }
            else
            {
                trafficSource = await _trafficSourceRepository.GetByIdAsync(id);
            }

            if (trafficSource == null)
            {
                throw new KeyNotFoundException($"Traffic source with ID {id} not found");
            }

            return _mapper.Map<TrafficSourceDto>(trafficSource);
        }

        public async Task<IEnumerable<TrafficSourceDto>> GetTrafficSourcesByPublisherIdAsync(int publisherId)
        {
            // First check if publisher exists
            if (!await _publisherRepository.ExistsAsync(p => p.PublisherId == publisherId))
            {
                throw new KeyNotFoundException($"Publisher with ID {publisherId} not found");
            }

            var trafficSources = await _trafficSourceRepository.GetAllAsync(ts => ts.PublisherId == publisherId);
            return _mapper.Map<IEnumerable<TrafficSourceDto>>(trafficSources);
        }

        public async Task<TrafficSourceDto> CreateTrafficSourceAsync(TrafficSourceCreateDto trafficSourceDto)
        {
            // Validate publisher exists if ID is provided
            if (trafficSourceDto.PublisherId.HasValue && !await _publisherRepository.ExistsAsync(p => p.PublisherId == trafficSourceDto.PublisherId))
            {
                throw new KeyNotFoundException($"Publisher with ID {trafficSourceDto.PublisherId} not found");
            }

            // Check if URL is valid if provided
            if (!string.IsNullOrWhiteSpace(trafficSourceDto.Url) && !Uri.IsWellFormedUriString(trafficSourceDto.Url, UriKind.Absolute))
            {
                throw new ArgumentException("The provided URL is not valid");
            }

            var trafficSource = _mapper.Map<TrafficSource>(trafficSourceDto);

            // Set default values
            trafficSource.AddedDate = DateOnly.FromDateTime(DateTime.UtcNow);
            trafficSource.IsActive = trafficSourceDto.IsActive ?? true;

            var createdTrafficSource = await _trafficSourceRepository.CreateAsync(trafficSource);
            return _mapper.Map<TrafficSourceDto>(createdTrafficSource);
        }

        public async Task<TrafficSourceDto> UpdateTrafficSourceAsync(TrafficSourceUpdateDto trafficSourceDto)
        {
            var existingTrafficSource = await _trafficSourceRepository.GetByIdAsync(trafficSourceDto.SourceId);
            if (existingTrafficSource == null)
            {
                throw new KeyNotFoundException($"Traffic source with ID {trafficSourceDto.SourceId} not found");
            }

            // Validate publisher exists if ID is provided
            if (trafficSourceDto.PublisherId.HasValue && !await _publisherRepository.ExistsAsync(p => p.PublisherId == trafficSourceDto.PublisherId))
            {
                throw new KeyNotFoundException($"Publisher with ID {trafficSourceDto.PublisherId} not found");
            }

            // Check if URL is valid if provided
            if (!string.IsNullOrWhiteSpace(trafficSourceDto.Url) && !Uri.IsWellFormedUriString(trafficSourceDto.Url, UriKind.Absolute))
            {
                throw new ArgumentException("The provided URL is not valid");
            }

            // Map updated properties while preserving ones we don't want to change
            _mapper.Map(trafficSourceDto, existingTrafficSource);

            await _trafficSourceRepository.UpdateAsync(existingTrafficSource);
            return _mapper.Map<TrafficSourceDto>(existingTrafficSource);
        }

        public async Task DeleteTrafficSourceAsync(int id)
        {
            var trafficSource = await _trafficSourceRepository.GetByIdAsync(id);
            if (trafficSource == null)
            {
                throw new KeyNotFoundException($"Traffic source with ID {id} not found");
            }

            // Check if this traffic source is used in any promoted URL variants
            if (trafficSource.PromoteUrlvariants.Any())
            {
                throw new InvalidOperationException("Cannot delete traffic source as it is used in promoted URL variants");
            }

            await _trafficSourceRepository.DeleteAsync(trafficSource);
        }

        public async Task<bool> ActivateTrafficSourceAsync(int id)
        {
            var trafficSource = await _trafficSourceRepository.GetByIdAsync(id);
            if (trafficSource == null)
            {
                throw new KeyNotFoundException($"Traffic source with ID {id} not found");
            }

            trafficSource.IsActive = true;
            await _trafficSourceRepository.UpdateAsync(trafficSource);
            return true;
        }

        public async Task<bool> DeactivateTrafficSourceAsync(int id)
        {
            var trafficSource = await _trafficSourceRepository.GetByIdAsync(id);
            if (trafficSource == null)
            {
                throw new KeyNotFoundException($"Traffic source with ID {id} not found");
            }

            trafficSource.IsActive = false;
            await _trafficSourceRepository.UpdateAsync(trafficSource);
            return true;
        }

        public async Task<bool> TrafficSourceExistsAsync(int id)
        {
            return await _trafficSourceRepository.ExistsAsync(ts => ts.SourceId == id);
        }

        public async Task<int> GetTrafficSourceCountAsync()
        {
            return await _trafficSourceRepository.CountAsync(ts => true);
        }

        public async Task<int> GetTrafficSourceCountByPublisherAsync(int publisherId)
        {
            return await _trafficSourceRepository.CountAsync(ts => ts.PublisherId == publisherId);
        }

        public async Task<IEnumerable<TrafficSourceDto>> GetActiveTrafficSourcesAsync()
        {
            var trafficSources = await _trafficSourceRepository.GetAllAsync(ts => ts.IsActive == true);
            return _mapper.Map<IEnumerable<TrafficSourceDto>>(trafficSources);
        }

        public async Task<IEnumerable<TrafficSourceDto>> GetTrafficSourcesByTypeAsync(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException("Type cannot be null or empty");
            }

            var trafficSources = await _trafficSourceRepository.GetAllAsync(ts => ts.Type == type);
            return _mapper.Map<IEnumerable<TrafficSourceDto>>(trafficSources);
        }
    }
}