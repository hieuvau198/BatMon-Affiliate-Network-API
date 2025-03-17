// Conversion Service Implementation
using Application.Contracts.Conversion;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ConversionService : IConversionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ConversionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<ConversionDto>> GetAllConversionsAsync()
        {
            var conversions = await _unitOfWork.Conversions.GetAllAsync();
            return _mapper.Map<IEnumerable<ConversionDto>>(conversions);
        }

        public async Task<ConversionDto> GetConversionByIdAsync(int id)
        {
            var conversion = await _unitOfWork.Conversions.GetByIdAsync(id);

            if (conversion == null)
            {
                throw new KeyNotFoundException($"Conversion with ID {id} not found");
            }

            return _mapper.Map<ConversionDto>(conversion);
        }

        public async Task<IEnumerable<ConversionDto>> GetConversionsByPromoteIdAsync(int promoteId)
        {
            var conversions = await _unitOfWork.Conversions.GetAllAsync(c => c.PromoteId == promoteId);
            return _mapper.Map<IEnumerable<ConversionDto>>(conversions);
        }

        public async Task<ConversionDto> CreateConversionAsync(ConversionCreateDto conversionDto)
        {
            // Validate that required fields are provided
            if (conversionDto.PromoteId == null)
            {
                throw new ArgumentException("PromoteId is required");
            }

            if (string.IsNullOrEmpty(conversionDto.CurrencyCode))
            {
                throw new ArgumentException("CurrencyCode is required");
            }

            var conversion = _mapper.Map<Conversion>(conversionDto);

            // Set default values
            conversion.ConversionTime = DateTime.UtcNow;
            conversion.Status = "Pending";
            conversion.IsUnique = true;
            conversion.IsSuspicious = false;
            conversion.IsFraud = false;

            var createdConversion = await _unitOfWork.Conversions.CreateAsync(conversion);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ConversionDto>(createdConversion);
        }

        public async Task<ConversionDto> UpdateConversionAsync(ConversionUpdateDto conversionDto)
        {
            var existingConversion = await _unitOfWork.Conversions.GetByIdAsync(conversionDto.ConversionId);
            if (existingConversion == null)
            {
                throw new KeyNotFoundException($"Conversion with ID {conversionDto.ConversionId} not found");
            }

            // Update approval date if status is changed to "Approved"
            if (conversionDto.Status == "Approved" && existingConversion.Status != "Approved")
            {
                existingConversion.ApprovalDate = DateOnly.FromDateTime(DateTime.UtcNow);
            }

            // Map updated properties
            _mapper.Map(conversionDto, existingConversion);

            await _unitOfWork.Conversions.UpdateAsync(existingConversion);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ConversionDto>(existingConversion);
        }

        public async Task DeleteConversionAsync(int id)
        {
            var conversion = await _unitOfWork.Conversions.GetByIdAsync(id);
            if (conversion == null)
            {
                throw new KeyNotFoundException($"Conversion with ID {id} not found");
            }

            await _unitOfWork.Conversions.DeleteAsync(conversion);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ConversionExistsAsync(int id)
        {
            return await _unitOfWork.Conversions.ExistsAsync(c => c.ConversionId == id);
        }
    }
}