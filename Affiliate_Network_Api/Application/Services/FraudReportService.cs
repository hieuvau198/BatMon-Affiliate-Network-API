using Application.Contracts.FraudReport;
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
    public class FraudReportService : IFraudReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FraudReportService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<FraudReportDto>> GetAllFraudReportsAsync()
        {
            var fraudReports = await _unitOfWork.FraudReports.GetAllAsync();
            return _mapper.Map<IEnumerable<FraudReportDto>>(fraudReports);
        }

        public async Task<FraudReportDto> GetFraudReportByIdAsync(int id, bool includeRelated = false)
        {
            FraudReport fraudReport;

            if (includeRelated)
            {
                fraudReport = await _unitOfWork.FraudReports.GetByIdAsync(id,
                    fr => fr.Campaign,
                    fr => fr.Publisher,
                    fr => fr.Advertiser);
            }
            else
            {
                fraudReport = await _unitOfWork.FraudReports.GetByIdAsync(id);
            }

            if (fraudReport == null)
            {
                throw new KeyNotFoundException($"Fraud Report with ID {id} not found");
            }

            return _mapper.Map<FraudReportDto>(fraudReport);
        }

        public async Task<IEnumerable<FraudReportDto>> GetFraudReportsByCampaignIdAsync(int campaignId)
        {
            var fraudReports = await _unitOfWork.FraudReports.GetAllAsync(fr => fr.CampaignId == campaignId);
            return _mapper.Map<IEnumerable<FraudReportDto>>(fraudReports);
        }

        public async Task<IEnumerable<FraudReportDto>> GetFraudReportsByPublisherIdAsync(int publisherId)
        {
            var fraudReports = await _unitOfWork.FraudReports.GetAllAsync(fr => fr.PublisherId == publisherId);
            return _mapper.Map<IEnumerable<FraudReportDto>>(fraudReports);
        }

        public async Task<IEnumerable<FraudReportDto>> GetFraudReportsByAdvertiserIdAsync(int advertiserId)
        {
            var fraudReports = await _unitOfWork.FraudReports.GetAllAsync(fr => fr.AdvertiserId == advertiserId);
            return _mapper.Map<IEnumerable<FraudReportDto>>(fraudReports);
        }

        public async Task<IEnumerable<FraudReportDto>> GetFraudReportsByStatusAsync(string status)
        {
            var fraudReports = await _unitOfWork.FraudReports.GetAllAsync(fr => fr.Status == status);
            return _mapper.Map<IEnumerable<FraudReportDto>>(fraudReports);
        }

        public async Task<IEnumerable<FraudReportDto>> GetUnreadFraudReportsAsync()
        {
            var fraudReports = await _unitOfWork.FraudReports.GetAllAsync(fr => fr.IsRead == false || fr.IsRead == null);
            return _mapper.Map<IEnumerable<FraudReportDto>>(fraudReports);
        }

        public async Task<FraudReportDto> CreateFraudReportAsync(FraudReportCreateDto fraudReportDto)
        {
            // Validate references if provided
            if (fraudReportDto.CampaignId.HasValue)
            {
                bool campaignExists = await _unitOfWork.Campaigns.ExistsAsync(c => c.CampaignId == fraudReportDto.CampaignId);
                if (!campaignExists)
                {
                    throw new KeyNotFoundException($"Campaign with ID {fraudReportDto.CampaignId} not found");
                }
            }

            if (fraudReportDto.PublisherId.HasValue)
            {
                bool publisherExists = await _unitOfWork.Publishers.ExistsAsync(p => p.PublisherId == fraudReportDto.PublisherId);
                if (!publisherExists)
                {
                    throw new KeyNotFoundException($"Publisher with ID {fraudReportDto.PublisherId} not found");
                }
            }

            if (fraudReportDto.AdvertiserId.HasValue)
            {
                bool advertiserExists = await _unitOfWork.Advertisers.ExistsAsync(a => a.AdvertiserId == fraudReportDto.AdvertiserId);
                if (!advertiserExists)
                {
                    throw new KeyNotFoundException($"Advertiser with ID {fraudReportDto.AdvertiserId} not found");
                }
            }

            var fraudReport = _mapper.Map<FraudReport>(fraudReportDto);

            // Set default values
            fraudReport.ReportDate = DateOnly.FromDateTime(DateTime.UtcNow);
            fraudReport.Status = "Pending"; // Default status
            fraudReport.IsRead = false;

            var createdFraudReport = await _unitOfWork.FraudReports.CreateAsync(fraudReport);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<FraudReportDto>(createdFraudReport);
        }

        public async Task<FraudReportDto> UpdateFraudReportAsync(FraudReportUpdateDto fraudReportDto)
        {
            var existingFraudReport = await _unitOfWork.FraudReports.GetByIdAsync(fraudReportDto.ReportId);
            if (existingFraudReport == null)
            {
                throw new KeyNotFoundException($"Fraud Report with ID {fraudReportDto.ReportId} not found");
            }

            // Map updated properties while preserving ones we don't want to change
            _mapper.Map(fraudReportDto, existingFraudReport);

            await _unitOfWork.FraudReports.UpdateAsync(existingFraudReport);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<FraudReportDto>(existingFraudReport);
        }

        public async Task DeleteFraudReportAsync(int id)
        {
            var fraudReport = await _unitOfWork.FraudReports.GetByIdAsync(id);
            if (fraudReport == null)
            {
                throw new KeyNotFoundException($"Fraud Report with ID {id} not found");
            }

            await _unitOfWork.FraudReports.DeleteAsync(fraudReport);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> MarkFraudReportAsReadAsync(int id)
        {
            var fraudReport = await _unitOfWork.FraudReports.GetByIdAsync(id);
            if (fraudReport == null)
            {
                throw new KeyNotFoundException($"Fraud Report with ID {id} not found");
            }

            fraudReport.IsRead = true;
            fraudReport.ReadDate = DateOnly.FromDateTime(DateTime.UtcNow);

            await _unitOfWork.FraudReports.UpdateAsync(fraudReport);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> MarkFraudReportAsUnreadAsync(int id)
        {
            var fraudReport = await _unitOfWork.FraudReports.GetByIdAsync(id);
            if (fraudReport == null)
            {
                throw new KeyNotFoundException($"Fraud Report with ID {id} not found");
            }

            fraudReport.IsRead = false;
            fraudReport.ReadDate = null;

            await _unitOfWork.FraudReports.UpdateAsync(fraudReport);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> FraudReportExistsAsync(int id)
        {
            return await _unitOfWork.FraudReports.ExistsAsync(fr => fr.ReportId == id);
        }

        public async Task<int> GetFraudReportCountAsync()
        {
            return await _unitOfWork.FraudReports.CountAsync(fr => true);
        }

        public async Task<int> GetUnreadFraudReportCountAsync()
        {
            return await _unitOfWork.FraudReports.CountAsync(fr => fr.IsRead == false || fr.IsRead == null);
        }

        public async Task<decimal> GetTotalFinancialImpactAsync(int? advertiserId = null, int? publisherId = null)
        {
            IEnumerable<FraudReport> fraudReports;

            if (advertiserId.HasValue && publisherId.HasValue)
            {
                fraudReports = await _unitOfWork.FraudReports.GetAllAsync(fr =>
                    fr.AdvertiserId == advertiserId && fr.PublisherId == publisherId);
            }
            else if (advertiserId.HasValue)
            {
                fraudReports = await _unitOfWork.FraudReports.GetAllAsync(fr => fr.AdvertiserId == advertiserId);
            }
            else if (publisherId.HasValue)
            {
                fraudReports = await _unitOfWork.FraudReports.GetAllAsync(fr => fr.PublisherId == publisherId);
            }
            else
            {
                fraudReports = await _unitOfWork.FraudReports.GetAllAsync();
            }

            return fraudReports.Sum(fr => fr.FinancialImpact ?? 0);
        }
    }
}