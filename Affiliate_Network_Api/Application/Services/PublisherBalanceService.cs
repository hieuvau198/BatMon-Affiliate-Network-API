using Application.Contracts.PublisherBalance;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PublisherBalanceService : IPublisherBalanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PublisherBalanceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<PublisherBalanceDto>> GetAllPublisherBalancesAsync()
        {
            var balances = await _unitOfWork.PublisherBalances.GetAllAsync(
                null,
                pb => pb.CurrencyCodeNavigation,
                pb => pb.Publisher
            );
            return _mapper.Map<IEnumerable<PublisherBalanceDto>>(balances);
        }

        public async Task<PublisherBalanceDto> GetPublisherBalanceByIdAsync(int id)
        {
            var balance = await _unitOfWork.PublisherBalances.GetByIdAsync(
                id,
                pb => pb.CurrencyCodeNavigation,
                pb => pb.Publisher
            );
            if (balance == null)
            {
                throw new KeyNotFoundException($"PublisherBalance with ID {id} not found");
            }
            return _mapper.Map<PublisherBalanceDto>(balance);
        }

        public async Task<PublisherBalanceDto> GetPublisherBalanceByPublisherIdAsync(int publisherId)
        {
            var balance = await _unitOfWork.PublisherBalances.FindAsync(
                pb => pb.PublisherId == publisherId,
                pb => pb.CurrencyCodeNavigation,
                pb => pb.Publisher
            );
            if (balance == null)
            {
                throw new KeyNotFoundException($"PublisherBalance for Publisher ID {publisherId} not found");
            }
            return _mapper.Map<PublisherBalanceDto>(balance);
        }

        public async Task<PublisherBalanceDto> CreatePublisherBalanceAsync(PublisherBalanceCreateDto publisherBalanceDto)
        {
            // Business logic checks
            if (publisherBalanceDto.AvailableBalance < 0 || publisherBalanceDto.PendingBalance < 0 || publisherBalanceDto.LifetimeEarnings < 0)
            {
                throw new ArgumentException("Balance amounts cannot be negative");
            }

            if (!await _unitOfWork.Publishers.ExistsAsync(p => p.PublisherId == publisherBalanceDto.PublisherId))
            {
                throw new ArgumentException($"Publisher with ID {publisherBalanceDto.PublisherId} does not exist");
            }

            if (!await _unitOfWork.Currencies.ExistsAsync(c => c.CurrencyCode == publisherBalanceDto.CurrencyCode))
            {
                throw new ArgumentException($"Currency code {publisherBalanceDto.CurrencyCode} is invalid");
            }

            if (await _unitOfWork.PublisherBalances.ExistsAsync(pb => pb.PublisherId == publisherBalanceDto.PublisherId))
            {
                throw new InvalidOperationException($"A balance already exists for Publisher ID {publisherBalanceDto.PublisherId}");
            }

            var publisherBalance = _mapper.Map<PublisherBalance>(publisherBalanceDto);
            publisherBalance.LastUpdated = DateOnly.FromDateTime(DateTime.UtcNow);

            var createdBalance = await _unitOfWork.PublisherBalances.CreateAsync(publisherBalance);
            await _unitOfWork.SaveChangesAsync();

            // Fetch with includes for DTO mapping
            var result = await _unitOfWork.PublisherBalances.GetByIdAsync(
                createdBalance.BalanceId,
                pb => pb.CurrencyCodeNavigation,
                pb => pb.Publisher
            );
            return _mapper.Map<PublisherBalanceDto>(result);
        }

        public async Task<PublisherBalanceDto> UpdatePublisherBalanceAsync(PublisherBalanceUpdateDto publisherBalanceDto)
        {
            var existingBalance = await _unitOfWork.PublisherBalances.GetByIdAsync(publisherBalanceDto.BalanceId);
            if (existingBalance == null)
            {
                throw new KeyNotFoundException($"PublisherBalance with ID {publisherBalanceDto.BalanceId} not found");
            }

            // Business logic checks
            if (publisherBalanceDto.AvailableBalance < 0 || publisherBalanceDto.PendingBalance < 0 || publisherBalanceDto.LifetimeEarnings < 0)
            {
                throw new ArgumentException("Balance amounts cannot be negative");
            }

            if (!await _unitOfWork.Publishers.ExistsAsync(p => p.PublisherId == publisherBalanceDto.PublisherId))
            {
                throw new ArgumentException($"Publisher with ID {publisherBalanceDto.PublisherId} does not exist");
            }

            if (!await _unitOfWork.Currencies.ExistsAsync(c => c.CurrencyCode == publisherBalanceDto.CurrencyCode))
            {
                throw new ArgumentException($"Currency code {publisherBalanceDto.CurrencyCode} is invalid");
            }

            _mapper.Map(publisherBalanceDto, existingBalance);
            existingBalance.LastUpdated = DateOnly.FromDateTime(DateTime.UtcNow);

            await _unitOfWork.PublisherBalances.UpdateAsync(existingBalance);
            await _unitOfWork.SaveChangesAsync();

            // Fetch with includes for DTO mapping
            var result = await _unitOfWork.PublisherBalances.GetByIdAsync(
                existingBalance.BalanceId,
                pb => pb.CurrencyCodeNavigation,
                pb => pb.Publisher
            );
            return _mapper.Map<PublisherBalanceDto>(result);
        }

        public async Task DeletePublisherBalanceAsync(int id)
        {
            var balance = await _unitOfWork.PublisherBalances.GetByIdAsync(id);
            if (balance == null)
            {
                throw new KeyNotFoundException($"PublisherBalance with ID {id} not found");
            }

            await _unitOfWork.PublisherBalances.DeleteAsync(balance);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> PublisherBalanceExistsAsync(int id)
        {
            return await _unitOfWork.PublisherBalances.ExistsAsync(pb => pb.BalanceId == id);
        }
    }
}