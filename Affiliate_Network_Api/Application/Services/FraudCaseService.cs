using Application.Contracts.FraudCase;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class FraudCaseService : IFraudCaseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FraudCaseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<FraudCaseDto>> GetAllFraudCasesAsync()
        {
            var fraudCases = await _unitOfWork.FraudCases.GetAllAsync(
                includes: new System.Linq.Expressions.Expression<Func<FraudCase, object>>[]
                {
                    fc => fc.Conversion,
                    fc => fc.FraudType
                });

            return _mapper.Map<IEnumerable<FraudCaseDto>>(fraudCases);
        }

        public async Task<FraudCaseDto> GetFraudCaseByIdAsync(int id)
        {
            var fraudCase = await _unitOfWork.FraudCases.GetByIdAsync(id,
                fc => fc.Conversion,
                fc => fc.FraudType);

            if (fraudCase == null)
            {
                throw new KeyNotFoundException($"FraudCase with ID {id} not found");
            }

            return _mapper.Map<FraudCaseDto>(fraudCase);
        }

        public async Task<FraudCaseDto> CreateFraudCaseAsync(FraudCaseCreateDto fraudCaseDto)
        {
            // Validation checks
            if (fraudCaseDto.ConversionId.HasValue)
            {
                var conversionExists = await _unitOfWork.Conversions.ExistsAsync(c => c.ConversionId == fraudCaseDto.ConversionId.Value);
                if (!conversionExists)
                {
                    throw new ArgumentException($"Conversion with ID {fraudCaseDto.ConversionId} does not exist");
                }
            }

            if (fraudCaseDto.FraudTypeId.HasValue)
            {
                var fraudTypeExists = await _unitOfWork.FraudTypes.ExistsAsync(ft => ft.FraudTypeId == fraudCaseDto.FraudTypeId.Value);
                if (!fraudTypeExists)
                {
                    throw new ArgumentException($"FraudType with ID {fraudCaseDto.FraudTypeId} does not exist");
                }
            }

            if (fraudCaseDto.DetectedBy.HasValue)
            {
                var adminExists = await _unitOfWork.Admins.ExistsAsync(a => a.AdminId == fraudCaseDto.DetectedBy.Value);
                if (!adminExists)
                {
                    throw new ArgumentException($"Admin with ID {fraudCaseDto.DetectedBy} does not exist");
                }
            }

            // Map DTO to entity
            var fraudCase = _mapper.Map<FraudCase>(fraudCaseDto);

            // Set default values
            fraudCase.DetectionDate = fraudCase.DetectionDate ?? DateOnly.FromDateTime(DateTime.UtcNow);
            fraudCase.Status = fraudCase.Status ?? "Open";

            // Create the fraud case
            var createdFraudCase = await _unitOfWork.FraudCases.CreateAsync(fraudCase);
            await _unitOfWork.SaveChangesAsync();

            // Fetch the created fraud case with related data for response
            var result = await _unitOfWork.FraudCases.GetByIdAsync(createdFraudCase.CaseId,
                fc => fc.Conversion,
                fc => fc.FraudType);

            return _mapper.Map<FraudCaseDto>(result);
        }

        public async Task<FraudCaseDto> UpdateFraudCaseAsync(FraudCaseUpdateDto fraudCaseDto)
        {
            var existingFraudCase = await _unitOfWork.FraudCases.GetByIdAsync(fraudCaseDto.CaseId,
                fc => fc.Conversion,
                fc => fc.FraudType);

            if (existingFraudCase == null)
            {
                throw new KeyNotFoundException($"FraudCase with ID {fraudCaseDto.CaseId} not found");
            }

            // Validation for status
            if (!string.IsNullOrEmpty(fraudCaseDto.Status))
            {
                var validStatuses = new[] { "Open", "InProgress", "Resolved", "Closed" };
                if (!validStatuses.Contains(fraudCaseDto.Status))
                {
                    throw new ArgumentException($"Invalid status value. Valid statuses are: {string.Join(", ", validStatuses)}");
                }

                if (fraudCaseDto.Status == "Resolved" || fraudCaseDto.Status == "Closed")
                {
                    if (string.IsNullOrWhiteSpace(fraudCaseDto.Resolution))
                    {
                        throw new ArgumentException("Resolution is required when status is set to Resolved or Closed");
                    }
                    if (!fraudCaseDto.ResolutionDate.HasValue)
                    {
                        fraudCaseDto.ResolutionDate = DateOnly.FromDateTime(DateTime.UtcNow);
                    }
                    if (!fraudCaseDto.ResolvedBy.HasValue)
                    {
                        throw new ArgumentException("ResolvedBy is required when status is set to Resolved or Closed");
                    }
                }
            }

            if (fraudCaseDto.ResolvedBy.HasValue)
            {
                var adminExists = await _unitOfWork.Admins.ExistsAsync(a => a.AdminId == fraudCaseDto.ResolvedBy.Value);
                if (!adminExists)
                {
                    throw new ArgumentException($"Admin with ID {fraudCaseDto.ResolvedBy} does not exist");
                }
            }

            // Map updated properties
            _mapper.Map(fraudCaseDto, existingFraudCase);

            await _unitOfWork.FraudCases.UpdateAsync(existingFraudCase);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<FraudCaseDto>(existingFraudCase);
        }

        public async Task DeleteFraudCaseAsync(int id)
        {
            var fraudCase = await _unitOfWork.FraudCases.GetByIdAsync(id);
            if (fraudCase == null)
            {
                throw new KeyNotFoundException($"FraudCase with ID {id} not found");
            }

            await _unitOfWork.FraudCases.DeleteAsync(fraudCase);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}